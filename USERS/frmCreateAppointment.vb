Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Drawing.Drawing2D

Public Class frmCreateAppointment

    Private selectedResidentID As Integer = 0
    Private authLetterBytes As Byte() = Nothing
    Private repIDBytes As Byte() = Nothing

    Public Class ServiceItem
        Public Property ServiceName As String
        Public Property DepartmentName As String

        Public Overrides Function ToString() As String
            Return ServiceName
        End Function
    End Class

    Private Sub frmCreateAppointment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cboRequestFor.Items.Clear()
        cboRequestFor.Items.AddRange({
            "Self",
            "Family Member / Relative",
            "Representative / On Behalf"
        })
        cboRequestFor.SelectedIndex = 0

        ToggleRepresentativeFields(False)

        LoadDepartments()
        LoadDocumentServices()

        lblControlNo.Text = GenerateControlNumber()
        LoadLoggedUserDefault()
    End Sub

    Private Sub cboRequestFor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRequestFor.SelectedIndexChanged
        Dim selectedOption As String = cboRequestFor.Text.Trim()

        If selectedOption = "Family Member / Relative" OrElse selectedOption = "Representative / On Behalf" Then
            ToggleRepresentativeFields(True)
        Else
            ToggleRepresentativeFields(False)
            txtNameOfRepresentative.Clear()
            authLetterBytes = Nothing
            repIDBytes = Nothing
            If picAuthLetter IsNot Nothing Then picAuthLetter.Image = Nothing
            If picRepID IsNot Nothing Then picRepID.Image = Nothing
        End If
    End Sub

    Private Sub ToggleRepresentativeFields(isVisible As Boolean)
        If lblRepName IsNot Nothing Then lblRepName.Visible = isVisible
        If txtNameOfRepresentative IsNot Nothing Then txtNameOfRepresentative.Visible = isVisible
        If lblAuthLetter IsNot Nothing Then lblAuthLetter.Visible = isVisible
        If picAuthLetter IsNot Nothing Then picAuthLetter.Visible = isVisible
        If lblRepID IsNot Nothing Then lblRepID.Visible = isVisible
        If picRepID IsNot Nothing Then picRepID.Visible = isVisible
    End Sub

    Private Sub picAuthLetter_DoubleClick(sender As Object, e As EventArgs) Handles picAuthLetter.DoubleClick
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            ofd.Title = "Select Authorization Letter Image"

            If ofd.ShowDialog() = DialogResult.OK Then
                authLetterBytes = File.ReadAllBytes(ofd.FileName)
                picAuthLetter.SizeMode = PictureBoxSizeMode.Zoom
                picAuthLetter.Image = Image.FromFile(ofd.FileName)
            End If
        End Using
    End Sub

    Private Sub picRepID_DoubleClick(sender As Object, e As EventArgs) Handles picRepID.DoubleClick
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            ofd.Title = "Select Representative Valid ID Image"

            If ofd.ShowDialog() = DialogResult.OK Then
                repIDBytes = File.ReadAllBytes(ofd.FileName)
                picRepID.SizeMode = PictureBoxSizeMode.Zoom
                picRepID.Image = Image.FromFile(ofd.FileName)
            End If
        End Using
    End Sub

    Private Sub LoadDepartments()
        cboDepartment.Items.Clear()
        Try
            connection()
            sql = "SELECT DepartmentName FROM departments WHERE IsActive = 1 ORDER BY DepartmentName ASC"
            cmd = New MySqlCommand(sql, cn)
            dr = cmd.ExecuteReader()
            While dr.Read()
                If Not IsDBNull(dr("DepartmentName")) Then
                    cboDepartment.Items.Add(dr("DepartmentName").ToString())
                End If
            End While
            dr.Close()
        Catch ex As Exception
            cboDepartment.Items.AddRange({"GENERAL SERVICES", "HEALTH OFFICE", "LUPONG TAGAPAMAYAPA", "SOCIAL SERVICES"})
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub LoadDocumentServices(Optional selectedDept As String = "")
        cboRequestType.Items.Clear()
        Try
            connection()
            sql = "SELECT ds.ServiceName, d.DepartmentName " &
                  "FROM document_services ds " &
                  "LEFT JOIN departments d ON ds.DepartmentID = d.DepartmentID " &
                  "WHERE (ds.IsActive = 1 OR ds.IsActive IS NULL) "

            If Not String.IsNullOrEmpty(selectedDept) Then
                sql &= "AND d.DepartmentName = @dept "
            End If

            sql &= "ORDER BY ds.ServiceName ASC"

            cmd = New MySqlCommand(sql, cn)

            If Not String.IsNullOrEmpty(selectedDept) Then
                cmd.Parameters.AddWithValue("@dept", selectedDept)
            End If

            dr = cmd.ExecuteReader()

            While dr.Read()
                Dim item As New ServiceItem With {
                    .ServiceName = dr("ServiceName").ToString(),
                    .DepartmentName = If(IsDBNull(dr("DepartmentName")), "", dr("DepartmentName").ToString())
                }
                cboRequestType.Items.Add(item)
            End While
            dr.Close()
        Catch ex As Exception
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub cboDepartment_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDepartment.SelectedIndexChanged
        If cboDepartment.SelectedItem IsNot Nothing Then
            LoadDocumentServices(cboDepartment.SelectedItem.ToString())
        Else
            LoadDocumentServices()
        End If
    End Sub

    Private Sub cboRequestType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRequestType.SelectedIndexChanged
        If cboRequestType.SelectedItem IsNot Nothing AndAlso TypeOf cboRequestType.SelectedItem Is ServiceItem Then
            Dim selectedItem As ServiceItem = CType(cboRequestType.SelectedItem, ServiceItem)
            If Not String.IsNullOrEmpty(selectedItem.DepartmentName) AndAlso (cboDepartment.SelectedItem Is Nothing OrElse cboDepartment.SelectedItem.ToString() <> selectedItem.DepartmentName) Then
                cboDepartment.SelectedItem = selectedItem.DepartmentName
            End If
        End If
    End Sub

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
        Finally
            CloseConnection()
        End Try

        Return newCtrlNo
    End Function

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
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub btnSelectUser_Click(sender As Object, e As EventArgs) Handles btnSelectUser.Click
        Using frm As New ResidenceList
            If frm.ShowDialog() = DialogResult.OK Then
                selectedResidentID = frm.SelectedResidentID
                txtName.Text = frm.SelectedFullName
                LoadSelectedResidentPicture(selectedResidentID)
            End If
        End Using
    End Sub

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

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If String.IsNullOrWhiteSpace(txtName.Text) Then
            MsgBox("Please select or enter a resident name.", MsgBoxStyle.Exclamation, "Validation Error")
            txtName.Focus()
            Return
        End If

        Dim isRepresentative As Boolean = (cboRequestFor.Text.Trim() = "Family Member / Relative" OrElse cboRequestFor.Text.Trim() = "Representative / On Behalf")

        If isRepresentative Then
            If String.IsNullOrWhiteSpace(txtNameOfRepresentative.Text) Then
                MsgBox("Please enter the Representative's full name.", MsgBoxStyle.Exclamation, "Validation Error")
                txtNameOfRepresentative.Focus()
                Return
            End If

            If authLetterBytes Is Nothing Then
                MsgBox("Please double-click the Authorization Letter box to upload the document image.", MsgBoxStyle.Exclamation, "Validation Error")
                Return
            End If

            If repIDBytes Is Nothing Then
                MsgBox("Please double-click the Representative ID box to upload the ID image.", MsgBoxStyle.Exclamation, "Validation Error")
                Return
            End If
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

        If String.IsNullOrWhiteSpace(txtPurpose.Text) Then
            MsgBox("Please state the purpose of your appointment.", MsgBoxStyle.Exclamation, "Validation Error")
            txtPurpose.Focus()
            Return
        End If

        Try
            connection()

            sql = "INSERT INTO appointments (ControlNo, ResidentID, FullName, RequestFor, RepresentativeName, AuthorizationLetter, RepresentativeIDCard, RequestType, Purpose, Department, DateSubmitted, ScheduledDate, Status, CreatedAt) " &
                  "VALUES (@ctrl, @resID, @name, @reqFor, @repName, @authLetter, @repID, @reqType, @purpose, @dept, NOW(), NOW(), 'PENDING', NOW())"

            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@ctrl", lblControlNo.Text.Trim())
            cmd.Parameters.AddWithValue("@resID", If(selectedResidentID > 0, selectedResidentID, DBNull.Value))
            cmd.Parameters.AddWithValue("@name", txtName.Text.Trim())
            cmd.Parameters.AddWithValue("@reqFor", cboRequestFor.Text.Trim())
            cmd.Parameters.AddWithValue("@repName", If(isRepresentative, txtNameOfRepresentative.Text.Trim(), DBNull.Value))
            cmd.Parameters.AddWithValue("@authLetter", If(isRepresentative AndAlso authLetterBytes IsNot Nothing, authLetterBytes, DBNull.Value))
            cmd.Parameters.AddWithValue("@repID", If(isRepresentative AndAlso repIDBytes IsNot Nothing, repIDBytes, DBNull.Value))
            cmd.Parameters.AddWithValue("@reqType", cboRequestType.Text.Trim())
            cmd.Parameters.AddWithValue("@purpose", txtPurpose.Text.Trim())
            cmd.Parameters.AddWithValue("@dept", cboDepartment.Text.Trim())

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