Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Drawing.Drawing2D

Public Class frmCreateAppointment

    Private selectedResidentID As Integer = 0

    Private Sub frmCreateAppointment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Setup "Request For" Options
        cboRequestFor.Items.Clear()
        cboRequestFor.Items.AddRange({
            "Self",
            "Family Member / Relative",
            "Representative / On Behalf"
        })
        cboRequestFor.SelectedIndex = 0

        ' Setup Document Service Types Dropdown
        cboRequestType.Items.Clear()
        cboRequestType.Items.AddRange({
            "Barangay Clearance",
            "Certificate of Indigency",
            "Barangay ID",
            "Business Permit",
            "First Time Job Seeker Certificate",
            "Certificate of Residency"
        })

        ' Setup Department Dropdown
        cboDepartment.Items.Clear()
        cboDepartment.Items.AddRange({
            "GENERAL SERVICES",
            "HEALTH OFFICE",
            "LUPONG TAGAPAMAYAPA",
            "SOCIAL SERVICES"
        })

        ' Set default Pick-up Date constraints (Prevent past dates)
        dtpAppointmentDate.MinDate = DateTime.Today

        ' Default to next weekday if today is a weekend
        Dim initialDate As DateTime = DateTime.Today
        While initialDate.DayOfWeek = DayOfWeek.Saturday OrElse initialDate.DayOfWeek = DayOfWeek.Sunday
            initialDate = initialDate.AddDays(1)
        End While
        dtpAppointmentDate.Value = initialDate

        ' Generate unique Control Number for Label
        lblControlNo.Text = GenerateControlNumber()

        ' Load logged user by default if available
        LoadLoggedUserDefault()
    End Sub

    ' --- PREVENT WEEKEND SELECTION ON PICKUP DATE ---
    Private Sub dtpAppointmentDate_ValueChanged(sender As Object, e As EventArgs) Handles dtpAppointmentDate.ValueChanged
        Dim selectedDate As DateTime = dtpAppointmentDate.Value

        If selectedDate.DayOfWeek = DayOfWeek.Saturday OrElse selectedDate.DayOfWeek = DayOfWeek.Sunday Then
            MsgBox("The Barangay Office is closed on weekends. Please select a weekday (Monday–Friday) for document pick-up.", MsgBoxStyle.Exclamation, "Invalid Pick-up Date")

            ' Auto-adjust to next Monday
            Dim adjustedDate As DateTime = selectedDate
            While adjustedDate.DayOfWeek = DayOfWeek.Saturday OrElse adjustedDate.DayOfWeek = DayOfWeek.Sunday
                adjustedDate = adjustedDate.AddDays(1)
            End While
            dtpAppointmentDate.Value = adjustedDate
        End If
    End Sub

    ' --- AUTO-GENERATE CONTROL NUMBER FOR LABEL ---
    Private Function GenerateControlNumber() As String
        Dim newCtrlNo As String = "APP-001"
        Try
            connection()
            sql = "SELECT ControlNo FROM appointments ORDER BY AppointmentID DESC LIMIT 1"
            cmd = New MySqlCommand(sql, cn)
            dr = cmd.ExecuteReader()

            If dr.Read() Then
                Dim lastCtrl As String = dr("ControlNo").ToString()
                Dim numPart As Integer = Convert.ToInt32(lastCtrl.Replace("APP-", ""))
                newCtrlNo = $"APP-{(numPart + 1):D3}"
            End If
            dr.Close()

        Catch ex As Exception
            ' Default fallback APP-001 on error
        Finally
            CloseConnection()
        End Try

        Return newCtrlNo
    End Function

    ' --- DEFAULT LOAD LOGGED USER PROFILE & PICTURE ---
    Private Sub LoadLoggedUserDefault()
        If String.IsNullOrEmpty(LoggedFullname) Then Return

        Try
            connection()
            sql = "SELECT ResidentID, FullName, Picture FROM residences WHERE FullName=@name OR Username=@name"
            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@name", LoggedFullname)
            dr = cmd.ExecuteReader()

            If dr.Read() Then
                selectedResidentID = If(IsDBNull(dr("ResidentID")), 0, Convert.ToInt32(dr("ResidentID")))
                txtName.Text = dr("FullName").ToString()

                If Not IsDBNull(dr("Picture")) Then
                    Dim imgBytes As Byte() = CType(dr("Picture"), Byte())
                    Using ms As New MemoryStream(imgBytes)
                        Dim rawImg As Image = Image.FromStream(ms)
                        If picUserProfile.Image IsNot Nothing Then picUserProfile.Image.Dispose()
                        picUserProfile.Image = MakeCircularImage(rawImg)
                    End Using
                End If
            End If
            dr.Close()

        Catch ex As Exception
            ' Keep blank if auto-load fails
        Finally
            CloseConnection()
        End Try
    End Sub

    ' --- SELECT USER (...) BUTTON CLICK HANDLER ---
    Private Sub btnSelectUser_Click(sender As Object, e As EventArgs) Handles btnSelectUser.Click
        Using frm As New ResidenceList
            If frm.ShowDialog() = DialogResult.OK Then
                selectedResidentID = frm.SelectedResidentID
                txtName.Text = frm.SelectedFullName
                LoadSelectedResidentPicture(selectedResidentID)
            End If
        End Using
    End Sub

    ' --- LOAD SELECTED USER PICTURE ---
    Private Sub LoadSelectedResidentPicture(resID As Integer)
        Try
            connection()
            sql = "SELECT Picture FROM residences WHERE ResidentID = @id"
            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@id", resID)
            dr = cmd.ExecuteReader()

            If dr.Read() AndAlso Not IsDBNull(dr("Picture")) Then
                Dim imgBytes As Byte() = CType(dr("Picture"), Byte())
                Using ms As New MemoryStream(imgBytes)
                    Dim rawImg As Image = Image.FromStream(ms)
                    If picUserProfile.Image IsNot Nothing Then picUserProfile.Image.Dispose()
                    picUserProfile.Image = MakeCircularImage(rawImg)
                End Using
            Else
                picUserProfile.Image = Nothing
            End If
            dr.Close()

        Catch ex As Exception
            picUserProfile.Image = Nothing
        Finally
            CloseConnection()
        End Try
    End Sub

    ' --- PERFECT CIRCULAR PROFILE PICTURE CROPPER ---
    Private Function MakeCircularImage(srcImage As Image) As Image
        Dim targetWidth As Integer = If(picUserProfile IsNot Nothing AndAlso picUserProfile.Width > 0, picUserProfile.Width, 100)
        Dim targetHeight As Integer = If(picUserProfile IsNot Nothing AndAlso picUserProfile.Height > 0, picUserProfile.Height, 100)
        Dim circleDiameter As Integer = Math.Min(targetWidth, targetHeight)

        Dim bmp As New Bitmap(circleDiameter, circleDiameter)

        Using g As Graphics = Graphics.FromImage(bmp)
            g.SmoothingMode = SmoothingMode.AntiAlias
            g.PixelOffsetMode = PixelOffsetMode.HighQuality
            g.CompositingQuality = CompositingQuality.HighQuality

            Using path As New GraphicsPath()
                path.AddEllipse(0, 0, circleDiameter, circleDiameter)
                g.SetClip(path)

                Dim minSrcDim As Integer = Math.Min(srcImage.Width, srcImage.Height)
                Dim srcRect As New Rectangle((srcImage.Width - minSrcDim) \ 2, (srcImage.Height - minSrcDim) \ 2, minSrcDim, minSrcDim)

                g.DrawImage(srcImage, New Rectangle(0, 0, circleDiameter, circleDiameter), srcRect, GraphicsUnit.Pixel)
            End Using
        End Using

        Return bmp
    End Function

    ' --- AUTOMATIC DEPARTMENT ROUTING ON REQUEST TYPE CHANGE ---
    Private Sub cboRequestType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRequestType.SelectedIndexChanged
        Select Case cboRequestType.Text.Trim()
            Case "Barangay Clearance", "Barangay ID", "Certificate of Residency"
                cboDepartment.SelectedItem = "GENERAL SERVICES"
            Case "Certificate of Indigency", "First Time Job Seeker Certificate"
                cboDepartment.SelectedItem = "SOCIAL SERVICES"
            Case "Business Permit"
                cboDepartment.SelectedItem = "GENERAL SERVICES"
        End Select
    End Sub

    ' --- SUBMIT PICK-UP APPOINTMENT TO DATABASE ---
    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        ' Validation checks
        If String.IsNullOrWhiteSpace(txtName.Text) Then
            MsgBox("Please select or enter a resident name.", MsgBoxStyle.Exclamation, "Validation Error")
            txtName.Focus()
            Return
        End If

        If cboRequestType.SelectedIndex = -1 AndAlso String.IsNullOrWhiteSpace(cboRequestType.Text) Then
            MsgBox("Please select a valid Request Type / Document Service.", MsgBoxStyle.Exclamation, "Validation Error")
            cboRequestType.Focus()
            Return
        End If

        If cboDepartment.SelectedIndex = -1 AndAlso String.IsNullOrWhiteSpace(cboDepartment.Text) Then
            MsgBox("Please select a Department.", MsgBoxStyle.Exclamation, "Validation Error")
            cboDepartment.Focus()
            Return
        End If

        ' Final Weekend Validation
        Dim pickupSchedule As DateTime = dtpAppointmentDate.Value.Date.Add(dtpAppointmentTime.Value.TimeOfDay)
        If pickupSchedule.DayOfWeek = DayOfWeek.Saturday OrElse pickupSchedule.DayOfWeek = DayOfWeek.Sunday Then
            MsgBox("Cannot schedule document pick-up on weekends. Please pick a weekday.", MsgBoxStyle.Exclamation, "Validation Error")
            Return
        End If

        If String.IsNullOrWhiteSpace(txtPurpose.Text) Then
            MsgBox("Please state the purpose of your appointment.", MsgBoxStyle.Exclamation, "Validation Error")
            txtPurpose.Focus()
            Return
        End If

        Try
            connection()

            sql = "INSERT INTO appointments (ControlNo, ResidentID, FullName, RequestType, Purpose, Department, DateSubmitted, ScheduledDate, Status, CreatedAt) " &
                  "VALUES (@ctrl, @resID, @name, @reqType, @purpose, @dept, NOW(), @schedDate, 'PENDING', NOW())"

            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@ctrl", lblControlNo.Text.Trim())
            cmd.Parameters.AddWithValue("@resID", If(selectedResidentID > 0, selectedResidentID, DBNull.Value))
            cmd.Parameters.AddWithValue("@name", txtName.Text.Trim())
            cmd.Parameters.AddWithValue("@reqType", cboRequestType.Text.Trim())
            cmd.Parameters.AddWithValue("@purpose", txtPurpose.Text.Trim())
            cmd.Parameters.AddWithValue("@dept", cboDepartment.Text.Trim())
            cmd.Parameters.AddWithValue("@schedDate", pickupSchedule)

            Dim rows As Integer = cmd.ExecuteNonQuery()
            If rows > 0 Then
                MsgBox($"Pick-up appointment request [{lblControlNo.Text.Trim()}] submitted successfully!", MsgBoxStyle.Information, "Success")
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Else
                MsgBox("Failed to submit pick-up appointment request.", MsgBoxStyle.Exclamation, "Warning")
            End If

        Catch ex As Exception
            MsgBox("Error scheduling pick-up appointment: " & ex.Message, MsgBoxStyle.Critical, "Database Error")
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class