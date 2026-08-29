Imports MySql.Data.MySqlClient
Imports System.IO

Public Class frmAppointmentDetails

    Private targetControlNo As String = ""

    ' Constructor accepting the Control No.
    Public Sub New(ByVal controlNo As String)
        InitializeComponent()
        targetControlNo = controlNo
    End Sub

    Private Sub frmAppointmentDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Setup Document Service Types Dropdown
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

    ' --- LOAD APPOINTMENT & RESIDENT INFORMATION ---
    Private Sub LoadAppointmentDetails()
        If String.IsNullOrEmpty(targetControlNo) Then Return

        Dim residentID As Integer = 0
        Dim fullName As String = ""

        Try
            connection()
            sql = "SELECT ControlNo, ResidentID, FullName, EmailAddress, PhoneNumber, FullAddress, " &
                  "RequestType, Status, DateSubmitted " &
                  "FROM appointments WHERE ControlNo = @ctrl"

            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@ctrl", targetControlNo)
            dr = cmd.ExecuteReader()

            If dr.Read() Then
                lblControlNo.Text = dr("ControlNo").ToString()
                fullName = dr("FullName").ToString()
                lblFullName.Text = fullName
                residentID = If(IsDBNull(dr("ResidentID")), 0, Convert.ToInt32(dr("ResidentID")))
                lblEmail.Text = If(IsDBNull(dr("EmailAddress")), "-", dr("EmailAddress").ToString())
                lblPhone.Text = If(IsDBNull(dr("PhoneNumber")), "-", dr("PhoneNumber").ToString())
                lblAddress.Text = If(IsDBNull(dr("FullAddress")), "-", dr("FullAddress").ToString())
                lblStatus.Text = dr("Status").ToString()

                If Not IsDBNull(dr("DateSubmitted")) Then
                    lblDateSubmitted.Text = Convert.ToDateTime(dr("DateSubmitted")).ToString("f")
                Else
                    lblDateSubmitted.Text = "-"
                End If

                ' Set selected Service Type in ComboBox
                Dim currentService As String = dr("RequestType").ToString()
                If cboServiceType.Items.Contains(currentService) Then
                    cboServiceType.SelectedItem = currentService
                Else
                    cboServiceType.Text = currentService
                End If
            End If
            dr.Close()

            ' Load Profile Picture Only
            LoadUserProfilePicture(residentID, fullName)

        Catch ex As Exception
            MsgBox("Error loading appointment details: " & ex.Message, MsgBoxStyle.Critical, "Database Error")
        Finally
            CloseConnection()
        End Try
    End Sub

    ' --- HELPER TO RETRIEVE PROFILE PICTURE ONLY ---
    Private Sub LoadUserProfilePicture(resID As Integer, name As String)
        Try
            Dim imgBytes As Byte() = Nothing

            ' 1. Search residences table for Picture
            sql = "SELECT Picture FROM residences WHERE ResidentID = @id OR FullName = @name"
            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@id", resID)
            cmd.Parameters.AddWithValue("@name", name)
            dr = cmd.ExecuteReader()

            If dr.Read() Then
                If Not IsDBNull(dr("Picture")) Then
                    imgBytes = CType(dr("Picture"), Byte())
                End If
            End If
            dr.Close()

            ' 2. Fallback search in users table for Picture
            If imgBytes Is Nothing Then
                sql = "SELECT Picture FROM users WHERE UserID = @id OR FullName = @name"
                cmd = New MySqlCommand(sql, cn)
                cmd.Parameters.AddWithValue("@id", resID)
                cmd.Parameters.AddWithValue("@name", name)
                dr = cmd.ExecuteReader()

                If dr.Read() Then
                    If Not IsDBNull(dr("Picture")) Then
                        imgBytes = CType(dr("Picture"), Byte())
                    End If
                End If
                dr.Close()
            End If

            ' Display Profile Picture in PictureBox
            If imgBytes IsNot Nothing AndAlso picProfile IsNot Nothing Then
                Using ms As New MemoryStream(imgBytes)
                    picProfile.SizeMode = PictureBoxSizeMode.Zoom
                    If picProfile.Image IsNot Nothing Then picProfile.Image.Dispose()
                    picProfile.Image = Image.FromStream(ms)
                End Using
            Else
                If picProfile IsNot Nothing Then picProfile.Image = Nothing
            End If

        Catch ex As Exception
            ' Keep PictureBox empty if profile picture fails to load
        End Try
    End Sub

    ' --- SAVE UPDATED DOCUMENT SERVICE ---
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If cboServiceType.SelectedIndex = -1 AndAlso String.IsNullOrWhiteSpace(cboServiceType.Text) Then
            MsgBox("Please select a valid Document Service Type.", MsgBoxStyle.Exclamation, "Validation Error")
            Return
        End If

        Dim updatedService As String = cboServiceType.Text.Trim()

        Try
            connection()
            sql = "UPDATE appointments SET RequestType = @reqType, UpdatedAt = NOW() WHERE ControlNo = @ctrl"
            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@reqType", updatedService)
            cmd.Parameters.AddWithValue("@ctrl", targetControlNo)

            Dim rows As Integer = cmd.ExecuteNonQuery()
            If rows > 0 Then
                MsgBox($"Document service for [{targetControlNo}] updated to '{updatedService}'!", MsgBoxStyle.Information, "Success")
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Else
                MsgBox("Failed to update service type.", MsgBoxStyle.Exclamation, "Warning")
            End If

        Catch ex As Exception
            MsgBox("Error saving updated service: " & ex.Message, MsgBoxStyle.Critical, "Database Error")
        Finally
            CloseConnection()
        End Try
    End Sub

    ' --- CLOSE FORM ---

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class