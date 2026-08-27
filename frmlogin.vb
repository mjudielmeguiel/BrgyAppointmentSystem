Imports MySql.Data.MySqlClient

Public Class frmlogin

    Private Sub frmlogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtUsername.Text = "Please Enter your username"
        txtUsername.ForeColor = Color.DarkGray
        txtPassword.Text = "Please Enter your Password"
        txtPassword.ForeColor = Color.DarkGray
        txtPassword.PasswordChar = Nothing
        lblError.Text = ""
        lblAttempts.Text = "0"
    End Sub

    Private Sub txtUsername_GotFocus(sender As Object, e As EventArgs) Handles txtUsername.GotFocus
        lblError.Text = ""
        If txtUsername.Text = "Please Enter your username" Then
            txtUsername.Clear()
            txtUsername.ForeColor = Color.Black
        End If
    End Sub

    Private Sub txtUsername_LostFocus(sender As Object, e As EventArgs) Handles txtUsername.LostFocus
        If String.IsNullOrWhiteSpace(txtUsername.Text) Then
            txtUsername.Text = "Please Enter your username"
            txtUsername.ForeColor = Color.DarkGray
        End If
    End Sub

    Private Sub txtPassword_GotFocus(sender As Object, e As EventArgs) Handles txtPassword.GotFocus
        lblError.Text = ""
        If txtPassword.Text = "Please Enter your Password" Then
            txtPassword.Clear()
            txtPassword.ForeColor = Color.Black
            txtPassword.PasswordChar = "●"c
        End If
    End Sub

    Private Sub txtPassword_LostFocus(sender As Object, e As EventArgs) Handles txtPassword.LostFocus
        If String.IsNullOrWhiteSpace(txtPassword.Text) Then
            txtPassword.Text = "Please Enter your Password"
            txtPassword.ForeColor = Color.DarkGray
            txtPassword.PasswordChar = Nothing
        End If
    End Sub

    Private Sub btnlogin_Click(sender As Object, e As EventArgs) Handles btnlogin.Click
        lblError.Text = ""
        Dim userInput As String = txtUsername.Text.Trim()
        Dim passInput As String = txtPassword.Text.Trim()

        If userInput = "" OrElse userInput = "Please Enter your username" OrElse
           passInput = "" OrElse passInput = "Please Enter your Password" Then
            lblError.Text = "Enter username and password."
            Return
        End If

        ProcessLogin(userInput, passInput)
    End Sub

    Private Sub ProcessLogin(userInput As String, passInput As String)
        Try
            connection()

            ' ==========================================================
            ' 1. CHECK ADMIN TABLE
            ' ==========================================================
            Dim adminSql As String = "SELECT Fullname, Password FROM admin WHERE Username=@user"
            Using cmdAdmin As New MySqlCommand(adminSql, cn)
                cmdAdmin.Parameters.AddWithValue("@user", userInput)
                Using drAdmin As MySqlDataReader = cmdAdmin.ExecuteReader()
                    If drAdmin.Read() Then
                        Dim dbAdminPass As String = drAdmin("Password").ToString()
                        Dim fullName As String = drAdmin("Fullname").ToString()

                        If dbAdminPass = passInput Then
                            LoggedFullname = fullName
                            LoggedRole = "Administrator"
                            lblError.Text = "✓ Admin Login Success! Redirecting..."
                            Timer1.Interval = 800
                            Timer1.Start()
                            Return
                        Else
                            lblError.Text = "✗ Incorrect Admin Password."
                            Return
                        End If
                    End If
                End Using
            End Using

            ' ==========================================================
            ' 2. CHECK USERS TABLE (Staff / System Users)
            ' ==========================================================
            Dim userSql As String = "SELECT FullName, Role, Password, LoginAttempts, AccountStatus FROM users WHERE Username=@user"
            Dim userFound As Boolean = False
            Dim uAccountStatus As String = ""
            Dim uAttempts As Integer = 0
            Dim uDbPass As String = ""
            Dim uFullName As String = ""
            Dim uRole As String = ""

            Using cmdUser As New MySqlCommand(userSql, cn)
                cmdUser.Parameters.AddWithValue("@user", userInput)
                Using drUser As MySqlDataReader = cmdUser.ExecuteReader()
                    If drUser.Read() Then
                        userFound = True
                        uAccountStatus = If(IsDBNull(drUser("AccountStatus")), "Active", drUser("AccountStatus").ToString().Trim())
                        uAttempts = If(IsDBNull(drUser("LoginAttempts")), 0, Convert.ToInt32(drUser("LoginAttempts")))
                        uDbPass = drUser("Password").ToString()
                        uFullName = drUser("FullName").ToString()
                        uRole = drUser("Role").ToString().Trim()
                    End If
                End Using
            End Using

            ' Process Users table results after DataReader is closed
            If userFound Then
                If uAccountStatus = "Locked" Then
                    lblError.Text = "Your account is locked/deactivated."
                    Return
                End If

                If uDbPass = passInput Then
                    LoggedFullname = uFullName
                    LoggedRole = uRole
                    ResetAttemptsAndLockout("users", userInput)

                    lblError.Text = "✓ Login Success! Redirecting..."
                    Timer1.Interval = 800
                    Timer1.Start()
                Else
                    uAttempts += 1
                    lblAttempts.Text = uAttempts.ToString()

                    If uAttempts >= 3 Then
                        LockoutAccount("users", userInput, uAttempts)
                        lblError.Text = "3 failed attempts reached. Account locked."
                    Else
                        UpdateAttemptCount("users", userInput, uAttempts)
                        lblError.Text = $"✗ Incorrect Password. Attempt {uAttempts} of 3."
                    End If
                End If
                Return
            End If

            ' ==========================================================
            ' 3. CHECK RESIDENCES TABLE (Resident Users)
            ' ==========================================================
            Dim resSql As String = "SELECT FullName, Password, LoginAttempts, AccountStatus FROM residences WHERE Username=@user"
            Dim resFound As Boolean = False
            Dim rAccountStatus As String = ""
            Dim rAttempts As Integer = 0
            Dim rDbPass As String = ""
            Dim rFullName As String = ""

            Using cmdRes As New MySqlCommand(resSql, cn)
                cmdRes.Parameters.AddWithValue("@user", userInput)
                Using drRes As MySqlDataReader = cmdRes.ExecuteReader()
                    If drRes.Read() Then
                        resFound = True
                        rAccountStatus = If(IsDBNull(drRes("AccountStatus")), "Active", drRes("AccountStatus").ToString().Trim())
                        rAttempts = If(IsDBNull(drRes("LoginAttempts")), 0, Convert.ToInt32(drRes("LoginAttempts")))
                        rDbPass = drRes("Password").ToString()
                        rFullName = drRes("FullName").ToString()
                    End If
                End Using
            End Using

            ' Process Residences table results after DataReader is closed
            If resFound Then
                If rAccountStatus = "Locked" Then
                    lblError.Text = "Your resident account is locked/deactivated."
                    Return
                End If

                If rDbPass = passInput Then
                    LoggedFullname = rFullName
                    LoggedRole = "Residence"
                    ResetAttemptsAndLockout("residences", userInput)

                    lblError.Text = "✓ Resident Login Success! Redirecting..."
                    Timer1.Interval = 800
                    Timer1.Start()
                Else
                    rAttempts += 1
                    lblAttempts.Text = rAttempts.ToString()

                    If rAttempts >= 3 Then
                        LockoutAccount("residences", userInput, rAttempts)
                        lblError.Text = "3 failed attempts reached. Resident account locked."
                    Else
                        UpdateAttemptCount("residences", userInput, rAttempts)
                        lblError.Text = $"✗ Incorrect Password. Attempt {rAttempts} of 3."
                    End If
                End If
                Return
            End If

            ' If username does not exist in all 3 tables
            lblError.Text = "User does not exist."

        Catch ex As Exception
            MsgBox("Login Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            CloseConnection()
        End Try
    End Sub

    ' --- HELPER FUNCTIONS FOR SECURITY UPDATES ---
    Private Sub ResetAttemptsAndLockout(tableName As String, user As String)
        Dim updateSql As String = $"UPDATE {tableName} SET LoginAttempts=0, AccountStatus='Active' WHERE Username=@user"
        Using cmd As New MySqlCommand(updateSql, cn)
            cmd.Parameters.AddWithValue("@user", user)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub UpdateAttemptCount(tableName As String, user As String, attempts As Integer)
        Dim updateSql As String = $"UPDATE {tableName} SET LoginAttempts=@attempts WHERE Username=@user"
        Using cmd As New MySqlCommand(updateSql, cn)
            cmd.Parameters.AddWithValue("@attempts", attempts)
            cmd.Parameters.AddWithValue("@user", user)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub LockoutAccount(tableName As String, user As String, attempts As Integer)
        Dim updateSql As String = $"UPDATE {tableName} SET LoginAttempts=@attempts, AccountStatus='Locked' WHERE Username=@user"
        Using cmd As New MySqlCommand(updateSql, cn)
            cmd.Parameters.AddWithValue("@attempts", attempts)
            cmd.Parameters.AddWithValue("@user", user)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub Timer1_Tick_1(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        Me.Hide()

        If String.Equals(LoggedRole, "Residence", StringComparison.OrdinalIgnoreCase) OrElse
           String.Equals(LoggedRole, "Resident", StringComparison.OrdinalIgnoreCase) Then
            frmResidenceMain.Show()
        Else
            frmMain.Show()
        End If
    End Sub

    Private Sub btnShowPass_Click(sender As Object, e As EventArgs) Handles btnShowPass.Click
        txtPassword.PasswordChar = If(txtPassword.PasswordChar = "●"c, Char.MinValue, "●"c)
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Me.Hide()
        frmcreateadmin.Show()
    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Application.Exit()
    End Sub
End Class