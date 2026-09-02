Imports MySql.Data.MySqlClient
Imports System.IO

Public Class frmAppointmentDetails

    Private targetControlNo As String = ""
    Private currentStatus As String = "PENDING"

    Public Sub New(ByVal controlNo As String)
        InitializeComponent()
        targetControlNo = controlNo
    End Sub

    Private Sub frmAppointmentDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RtbAddress.ReadOnly = True
        RtbAddress.BackColor = Color.FromArgb(245, 247, 250)
        RtbAddress.BorderStyle = BorderStyle.None

        ' Setup Service Dropdown
        cboServiceType.Items.Clear()
        cboServiceType.Items.AddRange({
            "Barangay Clearance",
            "Certificate of Indigency",
            "Barangay ID",
            "Business Permit",
            "First Time Job Seeker Certificate",
            "Certificate of Residency"
        })

        LoadAppointmentDetails()
    End Sub

    ' --- LOAD ALL APPOINTMENT, RESIDENT & REPRESENTATIVE INFO ---
    Private Sub LoadAppointmentDetails()
        If String.IsNullOrEmpty(targetControlNo) Then Return

        Dim residentID As Integer = 0
        Dim fullName As String = ""

        Try
            connection()

            ' Select appointment details including representative information
            sql = "SELECT ControlNo, ResidentID, FullName, EmailAddress, PhoneNumber, FullAddress, " &
                  "RequestType, Status, DateSubmitted, RequestFor, RepresentativeName, " &
                  "AuthorizationLetter, RepresentativeIDCard " &
                  "FROM appointments WHERE ControlNo = @ctrl"

            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@ctrl", targetControlNo)
            dr = cmd.ExecuteReader()

            If dr.Read() Then
                lblControlNo.Text = dr("ControlNo").ToString()
                fullName = dr("FullName").ToString()
                lblName.Text = fullName
                residentID = If(IsDBNull(dr("ResidentID")), 0, Convert.ToInt32(dr("ResidentID")))
                lblEmail.Text = If(IsDBNull(dr("EmailAddress")), "-", dr("EmailAddress").ToString())
                lblPhone.Text = If(IsDBNull(dr("PhoneNumber")), "-", dr("PhoneNumber").ToString())
                RtbAddress.Text = If(IsDBNull(dr("FullAddress")), "-", dr("FullAddress").ToString())

                currentStatus = dr("Status").ToString().ToUpper()
                lblStatus.Text = currentStatus

                If Not IsDBNull(dr("DateSubmitted")) Then
                    lblDateSubmitted.Text = Convert.ToDateTime(dr("DateSubmitted")).ToString("f")
                Else
                    lblDateSubmitted.Text = "-"
                End If

                Dim currentService As String = dr("RequestType").ToString()
                If cboServiceType.Items.Contains(currentService) Then
                    cboServiceType.SelectedItem = currentService
                Else
                    cboServiceType.Text = currentService
                End If

                ' Load Representative Information if applicable
                Dim requestFor As String = If(IsDBNull(dr("RequestFor")), "Self", dr("RequestFor").ToString())
                Dim repName As String = If(IsDBNull(dr("RepresentativeName")), "-", dr("RepresentativeName").ToString())

                If lblRequestFor IsNot Nothing Then lblRequestFor.Text = requestFor
                If lblRepresentativeName IsNot Nothing Then lblRepresentativeName.Text = repName

                ' Load Representative Images
                If Not IsDBNull(dr("AuthorizationLetter")) AndAlso picAuthLetter IsNot Nothing Then
                    DisplayImage(CType(dr("AuthorizationLetter"), Byte()), picAuthLetter)
                End If

                If Not IsDBNull(dr("RepresentativeIDCard")) AndAlso picRepID IsNot Nothing Then
                    DisplayImage(CType(dr("RepresentativeIDCard"), Byte()), picRepID)
                End If
            End If
            dr.Close()

            ' Fetch Profile & ID Images of Resident
            LoadResidentMedia(residentID, fullName)

            ' Dynamic Button Appearance Based on Status
            AdjustActionButtonsByStatus()

        Catch ex As Exception
            MsgBox("Error loading appointment details: " & ex.Message, MsgBoxStyle.Critical, "Database Error")
        Finally
            CloseConnection()
        End Try
    End Sub

    ' --- DYNAMICALLY CONFIGURE BUTTON TEXT ---
    Private Sub AdjustActionButtonsByStatus()
        Select Case currentStatus
            Case "PENDING"
                btnApprove.Text = "Approve"
                btnApprove.Visible = True

                btnReject.Text = "Reject"
                btnReject.Visible = True

            Case "APPROVED"
                btnApprove.Text = "Complete"
                btnApprove.Visible = True

                btnReject.Text = "Cancel"
                btnReject.Visible = True

            Case "REJECTED"
                btnApprove.Text = "Re-Apply"
                btnApprove.Visible = True
                btnReject.Visible = False

            Case Else ' COMPLETED, CANCELLED
                btnApprove.Visible = False
                btnReject.Visible = False
        End Select
    End Sub

    ' --- TOP BUTTON (APPROVE / COMPLETE / RE-APPLY) ---
    Private Sub btnApprove_Click(sender As Object, e As EventArgs) Handles btnApprove.Click
        If currentStatus = "PENDING" Then
            If MsgBox($"Are you sure you want to APPROVE appointment [{targetControlNo}]?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Confirm Approval") = MsgBoxResult.Yes Then
                UpdateAppointmentStatus("APPROVED")
            End If

        ElseIf currentStatus = "APPROVED" Then
            If MsgBox($"Mark appointment [{targetControlNo}] as COMPLETED?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Confirm Completion") = MsgBoxResult.Yes Then
                UpdateAppointmentStatus("COMPLETED")
            End If

        ElseIf currentStatus = "REJECTED" Then
            If MsgBox($"Resubmit appointment [{targetControlNo}] to PENDING?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Confirm Re-Apply") = MsgBoxResult.Yes Then
                UpdateAppointmentStatus("PENDING")
            End If
        End If
    End Sub

    ' --- BOTTOM BUTTON (REJECT / CANCEL) ---
    Private Sub btnReject_Click(sender As Object, e As EventArgs) Handles btnReject.Click
        If currentStatus = "PENDING" Then
            If MsgBox($"Are you sure you want to REJECT appointment [{targetControlNo}]?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, "Confirm Rejection") = MsgBoxResult.Yes Then
                UpdateAppointmentStatus("REJECTED")
            End If

        ElseIf currentStatus = "APPROVED" Then
            If MsgBox($"Are you sure you want to CANCEL appointment [{targetControlNo}]?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, "Confirm Cancellation") = MsgBoxResult.Yes Then
                UpdateAppointmentStatus("CANCELLED")
            End If
        End If
    End Sub

    ' --- UPDATE STATUS IN MYSQL ---
    Private Sub UpdateAppointmentStatus(newStatus As String)
        Try
            connection()

            sql = "UPDATE appointments SET Status = @status, RequestType = @reqType, UpdatedAt = NOW() WHERE ControlNo = @ctrl"
            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@status", newStatus)
            cmd.Parameters.AddWithValue("@reqType", cboServiceType.Text.Trim())
            cmd.Parameters.AddWithValue("@ctrl", targetControlNo)

            Dim rows As Integer = cmd.ExecuteNonQuery()
            If rows > 0 Then
                MsgBox($"Appointment [{targetControlNo}] successfully updated to {newStatus}!", MsgBoxStyle.Information, "Status Updated")
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Else
                MsgBox("Failed to update status.", MsgBoxStyle.Exclamation, "Warning")
            End If

        Catch ex As Exception
            MsgBox("Error updating status: " & ex.Message, MsgBoxStyle.Critical, "Database Error")
        Finally
            CloseConnection()
        End Try
    End Sub

    ' --- LOAD RESIDENT MEDIA ---
    Private Sub LoadResidentMedia(resID As Integer, name As String)
        Try
            sql = "SELECT Picture, IdentificationFront, IdentificationBack FROM residences " &
                  "WHERE ResidentID = @id OR FullName = @name"

            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@id", resID)
            cmd.Parameters.AddWithValue("@name", name)
            dr = cmd.ExecuteReader()

            If dr.Read() Then
                If Not IsDBNull(dr("Picture")) AndAlso picProfile IsNot Nothing Then
                    DisplayImage(CType(dr("Picture"), Byte()), picProfile)
                End If

                If Not IsDBNull(dr("IdentificationFront")) AndAlso picIDFront IsNot Nothing Then
                    DisplayImage(CType(dr("IdentificationFront"), Byte()), picIDFront)
                End If

                If Not IsDBNull(dr("IdentificationBack")) AndAlso picIDBack IsNot Nothing Then
                    DisplayImage(CType(dr("IdentificationBack"), Byte()), picIDBack)
                End If
            End If
            dr.Close()

        Catch ex As Exception
        End Try
    End Sub

    Private Sub DisplayImage(imgBytes As Byte(), picBox As PictureBox)
        If imgBytes IsNot Nothing AndAlso imgBytes.Length > 0 Then
            Using ms As New MemoryStream(imgBytes)
                picBox.SizeMode = PictureBoxSizeMode.Zoom
                If picBox.Image IsNot Nothing Then picBox.Image.Dispose()
                picBox.Image = Image.FromStream(ms)
            End Using
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class