Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Drawing.Imaging
Imports System.Text.RegularExpressions

Public Class frmUserInformation

    Private currentUserID As Integer
    Private originalPassword As String = String.Empty
    Private userImageBytes As Byte() = Nothing
    Private isAdminUser As Boolean = False
    Private allowedRoles As New List(Of String) From {
        "Administrator", "System Admin", "Manager", "Supervisor",
        "Staff", "Office Staff", "Encoder", "Clerk",
        "Security", "Security Guard", "Receptionist",
        "Cashier", "Accounting", "Auditor", "HR Staff",
        "IT Support", "Technician", "Maintenance",
        "Driver", "Inventory Staff", "Nurse", "Medical Staff"
    }

    Public Sub New(ByVal userID As Integer)
        InitializeComponent()
        currentUserID = userID
    End Sub

    Private Sub frmUserInformation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.TopMost = True
        CheckIfAdmin()
        LoadRoles()
        LoadUserData()
        ClearAllErrors()
    End Sub

    Private Sub CheckIfAdmin()
        Try
            connection()
            sql = "SELECT AdminID FROM admin WHERE FullName = @name"
            Using localCmd As New MySqlCommand(sql, cn)
                localCmd.Parameters.AddWithValue("@name", LoggedFullname)
                Using localDr As MySqlDataReader = localCmd.ExecuteReader()
                    If localDr.Read() Then
                        isAdminUser = True
                    End If
                End Using
            End Using

            If Not isAdminUser Then
                lblStaffCode.Enabled = False
                txtLastname.ReadOnly = True
                txtFirstname.ReadOnly = True
                txtEmail.ReadOnly = True
                cboRole.Enabled = False
                txtUsername.ReadOnly = True
                txtPassword.ReadOnly = True
                txtConfirmPass.ReadOnly = True
                picUser.Enabled = False
                btnSubmit.Visible = False
                BtnDelete.Visible = False
            End If

        Catch ex As Exception
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub LoadRoles()
        cboRole.Items.AddRange(allowedRoles.ToArray())
    End Sub

    Private Sub LoadUserData()
        Try
            connection()
            Dim query As String = "SELECT StaffCode, Lastname, Firstname, Email, Role, Username, Password, AccountStatus, Picture FROM users WHERE UserID = @uid"

            Dim dt As New DataTable()
            Using localCmd As New MySqlCommand(query, cn)
                localCmd.Parameters.AddWithValue("@uid", currentUserID)
                Using adapter As New MySqlDataAdapter(localCmd)
                    adapter.Fill(dt)
                End Using
            End Using

            If dt.Rows.Count > 0 Then
                Dim row As DataRow = dt.Rows(0)
                lblStaffCode.Text = If(row("StaffCode") Is DBNull.Value, "", row("StaffCode").ToString())
                txtLastname.Text = If(row("Lastname") Is DBNull.Value, "", row("Lastname").ToString())
                txtFirstname.Text = If(row("Firstname") Is DBNull.Value, "", row("Firstname").ToString())
                txtEmail.Text = If(row("Email") Is DBNull.Value, "", row("Email").ToString())
                cboRole.Text = If(row("Role") Is DBNull.Value, "", row("Role").ToString())
                txtUsername.Text = If(row("Username") Is DBNull.Value, "", row("Username").ToString())

                originalPassword = If(row("Password") Is DBNull.Value, "", row("Password").ToString())
                txtPassword.Text = originalPassword
                txtConfirmPass.Text = originalPassword

                lblAccountStatus.Text = "Status: " & If(row("AccountStatus") Is DBNull.Value, "Active", row("AccountStatus").ToString())

                If row("Picture") IsNot DBNull.Value Then
                    userImageBytes = CType(row("Picture"), Byte())

                    If userImageBytes.Length > 0 Then
                        Try
                            Using ms As New MemoryStream(userImageBytes)
                                picUser.SizeMode = PictureBoxSizeMode.StretchImage
                                picUser.Image = Image.FromStream(ms)
                            End Using
                        Catch exStream As Exception
                            picUser.Image = Nothing
                            userImageBytes = Nothing
                        End Try
                    Else
                        picUser.Image = Nothing
                        userImageBytes = Nothing
                    End If
                Else
                    picUser.Image = Nothing
                    userImageBytes = Nothing
                End If
            Else
                MsgBox("User record not found.", MsgBoxStyle.Exclamation)
            End If

        Catch ex As Exception
            MsgBox("Error loading user information: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub ClearAllErrors()
        lblLastnameError.Text = ""
        lblFirstnameError.Text = ""
        lblEmailError.Text = ""
        lblUsernameError.Text = ""
        lblConfirmPassError.Text = ""
        lblRoleError.Text = ""
    End Sub

    Private Sub txtLastname_TextChanged(sender As Object, e As EventArgs) Handles txtLastname.TextChanged
        If String.IsNullOrWhiteSpace(txtLastname.Text) Then
            lblLastnameError.Text = "Lastname is required."
            lblLastnameError.ForeColor = Color.Red
        ElseIf Not Regex.IsMatch(txtLastname.Text.Trim(), "^[a-zA-ZñÑ\s]+$") Then
            lblLastnameError.Text = "Letters only (no numbers/symbols)."
            lblLastnameError.ForeColor = Color.Red
        Else
            lblLastnameError.Text = ""
        End If
    End Sub

    Private Sub txtFirstname_TextChanged(sender As Object, e As EventArgs) Handles txtFirstname.TextChanged
        If String.IsNullOrWhiteSpace(txtFirstname.Text) Then
            lblFirstnameError.Text = "Firstname is required."
            lblFirstnameError.ForeColor = Color.Red
        ElseIf Not Regex.IsMatch(txtFirstname.Text.Trim(), "^[a-zA-ZñÑ\s]+$") Then
            lblFirstnameError.Text = "Letters only (no numbers/symbols)."
            lblFirstnameError.ForeColor = Color.Red
        Else
            lblFirstnameError.Text = ""
        End If
    End Sub

    Private Sub txtEmail_TextChanged(sender As Object, e As EventArgs) Handles txtEmail.TextChanged
        If String.IsNullOrWhiteSpace(txtEmail.Text) Then
            lblEmailError.Text = "Email is required."
            lblEmailError.ForeColor = Color.Red
            Return
        End If

        Dim gmailPattern As String = "^[a-zA-Z0-9._%+-]+@gmail\.com$"
        If Not Regex.IsMatch(txtEmail.Text.Trim(), gmailPattern, RegexOptions.IgnoreCase) Then
            lblEmailError.Text = "Email must end strictly with @gmail.com."
            lblEmailError.ForeColor = Color.Red
            Return
        End If

        If Not isAdminUser Then Return

        Try
            connection()
            sql = "SELECT COUNT(*) FROM users WHERE Email = @email AND UserID <> @uid"
            Using localCmd As New MySqlCommand(sql, cn)
                localCmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim())
                localCmd.Parameters.AddWithValue("@uid", currentUserID)
                Dim count As Integer = Convert.ToInt32(localCmd.ExecuteScalar())

                If count > 0 Then
                    lblEmailError.Text = "Email already exists!"
                    lblEmailError.ForeColor = Color.Red
                Else
                    lblEmailError.Text = ""
                End If
            End Using
        Catch ex As Exception
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub cboRole_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRole.SelectedIndexChanged, cboRole.TextChanged
        If Not allowedRoles.Contains(cboRole.Text.Trim()) Then
            lblRoleError.Text = "This role is not related in this record"
            lblRoleError.ForeColor = Color.Red
        Else
            lblRoleError.Text = ""
        End If
    End Sub

    Private Sub txtUsername_TextChanged(sender As Object, e As EventArgs) Handles txtUsername.TextChanged
        If String.IsNullOrWhiteSpace(txtUsername.Text) Then
            lblUsernameError.Text = "Username is required."
            lblUsernameError.ForeColor = Color.Red
            Return
        End If

        Dim lowerUser As String = txtUsername.Text.Trim().ToLower()
        If lowerUser.Contains("admin") Then
            lblUsernameError.Text = "Usernames related to 'admin' are strictly prohibited!"
            lblUsernameError.ForeColor = Color.Red
            Return
        End If

        If Not isAdminUser Then Return

        Try
            connection()

            sql = "SELECT COUNT(*) FROM users WHERE Username = @uname AND UserID <> @uid"
            Using localCmd As New MySqlCommand(sql, cn)
                localCmd.Parameters.AddWithValue("@uname", txtUsername.Text.Trim())
                localCmd.Parameters.AddWithValue("@uid", currentUserID)
                Dim userCount As Integer = Convert.ToInt32(localCmd.ExecuteScalar())

                If userCount > 0 Then
                    lblUsernameError.Text = "Username already exists"
                    lblUsernameError.ForeColor = Color.Red
                    Return
                End If
            End Using

            Dim adminSql As String = "SELECT COUNT(*) FROM admin WHERE Username = @uname"
            Using adminCmd As New MySqlCommand(adminSql, cn)
                adminCmd.Parameters.AddWithValue("@uname", txtUsername.Text.Trim())
                Dim adminCount As Integer = Convert.ToInt32(adminCmd.ExecuteScalar())

                If adminCount > 0 Then
                    lblUsernameError.Text = "Username already exists"
                    lblUsernameError.ForeColor = Color.Red
                    Return
                End If
            End Using

            lblUsernameError.Text = ""

        Catch ex As Exception
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub txtPassword_TextChanged(sender As Object, e As EventArgs) Handles txtPassword.TextChanged, txtConfirmPass.TextChanged
        ' If password hasn't changed from original, skip validation errors entirely
        If txtPassword.Text = originalPassword Then
            lblConfirmPassError.Text = ""
            Return
        End If

        Dim hasUpper As Boolean = txtPassword.Text.Any(AddressOf Char.IsUpper)
        Dim hasLower As Boolean = txtPassword.Text.Any(AddressOf Char.IsLower)

        If String.IsNullOrWhiteSpace(txtPassword.Text) Then
            lblConfirmPassError.Text = "Password is required."
            lblConfirmPassError.ForeColor = Color.Red
            Return
        End If

        If Not hasUpper OrElse Not hasLower OrElse txtPassword.Text.Length < 6 Then
            lblConfirmPassError.Text = "Password must be at least 6 chars with uppercase and lowercase."
            lblConfirmPassError.ForeColor = Color.Red
            Return
        End If

        If txtPassword.Text.Trim() <> txtConfirmPass.Text.Trim() Then
            lblConfirmPassError.Text = "Passwords do not match."
            lblConfirmPassError.ForeColor = Color.Red
        Else
            lblConfirmPassError.Text = "Passwords match."
            lblConfirmPassError.ForeColor = Color.Green
        End If
    End Sub

    Private Sub picUser_DoubleClick(sender As Object, e As EventArgs) Handles picUser.DoubleClick
        If Not isAdminUser Then Return

        Using ofd As New OpenFileDialog()
            ofd.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            If ofd.ShowDialog() = DialogResult.OK Then
                picUser.SizeMode = PictureBoxSizeMode.StretchImage
                picUser.Image = Image.FromFile(ofd.FileName)

                Using ms As New MemoryStream()
                    picUser.Image.Save(ms, ImageFormat.Jpeg)
                    userImageBytes = ms.ToArray()
                End Using
            End If
        End Using
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If Not isAdminUser Then
            MsgBox("Only Admin can update user information!", MsgBoxStyle.Exclamation)
            Return
        End If

        If String.IsNullOrWhiteSpace(txtLastname.Text) OrElse
           String.IsNullOrWhiteSpace(txtFirstname.Text) OrElse
           String.IsNullOrWhiteSpace(txtEmail.Text) OrElse
           String.IsNullOrWhiteSpace(txtUsername.Text) OrElse
           String.IsNullOrWhiteSpace(txtPassword.Text) OrElse
           String.IsNullOrWhiteSpace(txtConfirmPass.Text) OrElse
           cboRole.SelectedIndex = -1 Then
            MsgBox("Please fill all fields correctly!", MsgBoxStyle.Exclamation)
            Return
        End If

        If txtUsername.Text.Trim().ToLower().Contains("admin") Then
            MsgBox("Usernames related to 'admin' are strictly prohibited!", MsgBoxStyle.Critical)
            Return
        End If

        If Not allowedRoles.Contains(cboRole.Text.Trim()) Then
            MsgBox("Selected role is invalid or not related to this record!", MsgBoxStyle.Exclamation)
            Return
        End If

        If Not Regex.IsMatch(txtLastname.Text.Trim(), "^[a-zA-ZñÑ\s]+$") OrElse Not Regex.IsMatch(txtFirstname.Text.Trim(), "^[a-zA-ZñÑ\s]+$") Then
            MsgBox("First name and Last name must contain letters only!", MsgBoxStyle.Exclamation)
            Return
        End If

        If txtPassword.Text <> originalPassword Then
            Dim hasUpper As Boolean = txtPassword.Text.Any(AddressOf Char.IsUpper)
            Dim hasLower As Boolean = txtPassword.Text.Any(AddressOf Char.IsLower)

            If Not hasUpper OrElse Not hasLower OrElse txtPassword.Text.Length < 6 Then
                MsgBox("Password must be at least 6 characters and contain both uppercase and lowercase letters!", MsgBoxStyle.Exclamation)
                Return
            End If

            If txtPassword.Text.Trim() <> txtConfirmPass.Text.Trim() Then
                MsgBox("Password does not match!", MsgBoxStyle.Exclamation)
                Return
            End If
        End If

        Dim gmailPattern As String = "^[a-zA-Z0-9._%+-]+@gmail\.com$"
        If Not Regex.IsMatch(txtEmail.Text.Trim(), gmailPattern, RegexOptions.IgnoreCase) Then
            MsgBox("Email must be a valid @gmail.com address without extra characters!", MsgBoxStyle.Exclamation)
            Return
        End If

        If MsgBox("Are you sure you want to update this user information?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Confirm Update") = MsgBoxResult.No Then
            Return
        End If

        Dim fullName As String = $"{txtLastname.Text.Trim()}, {txtFirstname.Text.Trim()}"

        Try
            connection()
            sql = "UPDATE users SET Lastname = @lname, Firstname = @fname, FullName = @fullname, Email = @email, Username = @uname, Password = @pass, Role = @role, Picture = @pic WHERE UserID = @uid"
            Using localCmd As New MySqlCommand(sql, cn)
                localCmd.Parameters.AddWithValue("@lname", txtLastname.Text.Trim())
                localCmd.Parameters.AddWithValue("@fname", txtFirstname.Text.Trim())
                localCmd.Parameters.AddWithValue("@fullname", fullName)
                localCmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim())
                localCmd.Parameters.AddWithValue("@uname", txtUsername.Text.Trim())
                localCmd.Parameters.AddWithValue("@pass", txtPassword.Text)
                localCmd.Parameters.AddWithValue("@role", cboRole.Text)

                If userImageBytes IsNot Nothing Then
                    localCmd.Parameters.Add("@pic", MySqlDbType.LongBlob).Value = userImageBytes
                Else
                    localCmd.Parameters.Add("@pic", MySqlDbType.LongBlob).Value = DBNull.Value
                End If

                localCmd.Parameters.AddWithValue("@uid", currentUserID)
                localCmd.ExecuteNonQuery()
            End Using

            MsgBox("User information updated successfully!", MsgBoxStyle.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        If Not isAdminUser Then
            MsgBox("Only Admin can delete users!", MsgBoxStyle.Exclamation)
            Return
        End If

        If MsgBox("Are you sure you want to delete this user?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Confirm Delete") = MsgBoxResult.Yes Then
            Try
                connection()
                sql = "DELETE FROM users WHERE UserID = @uid"
                Using localCmd As New MySqlCommand(sql, cn)
                    localCmd.Parameters.AddWithValue("@uid", currentUserID)
                    localCmd.ExecuteNonQuery()
                End Using

                MsgBox("User deleted successfully!", MsgBoxStyle.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As Exception
                MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
            Finally
                CloseConnection()
            End Try
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class