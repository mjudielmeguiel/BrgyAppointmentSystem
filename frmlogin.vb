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

            Dim adminSql As String = "SELECT * FROM admin WHERE Username=@user"
            Using cmdAdmin As New MySqlCommand(adminSql, cn)
                cmdAdmin.Parameters.AddWithValue("@user", userInput)
                Using drAdmin As MySqlDataReader = cmdAdmin.ExecuteReader()
                    If drAdmin.Read() Then
                        Dim dbAdminPass As String = drAdmin("Password").ToString()
                        If dbAdminPass = passInput Then
                            LoggedFullname = drAdmin("FullName").ToString()
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

            Dim userSql As String = "SELECT UserID, FullName, Role, Password, LoginAttempts, AccountStatus FROM users WHERE Username=@user"
            Using cmdUser As New MySqlCommand(userSql, cn)
                cmdUser.Parameters.AddWithValue("@user", userInput)
                Using drUser As MySqlDataReader = cmdUser.ExecuteReader()
                    If drUser.Read() Then
                        Dim accountStatus As String = drUser("AccountStatus").ToString().Trim()
                        Dim currentAttempts As Integer = Convert.ToInt32(drUser("LoginAttempts"))
                        Dim dbUserPass As String = drUser("Password").ToString()
                        Dim fullName As String = drUser("FullName").ToString()
                        Dim role As String = drUser("Role").ToString().Trim()

                        If accountStatus = "Locked" Then
                            lblError.Text = "Your account is locked/deactivated."
                            Return
                        End If

                        drUser.Close()

                        If dbUserPass = passInput Then
                            LoggedFullname = fullName
                            LoggedRole = role
                            ResetAttemptsAndLockout(userInput)

                            lblError.Text = "✓ Login Success! Redirecting..."
                            Timer1.Interval = 800
                            Timer1.Start()
                        Else
                            currentAttempts += 1
                            lblAttempts.Text = currentAttempts.ToString()

                            If currentAttempts >= 3 Then
                                LockoutAccount(userInput, currentAttempts)
                                lblError.Text = "3 failed attempts reached. Account locked."
                            Else
                                UpdateAttemptCount(userInput, currentAttempts)
                                lblError.Text = $"✗ Incorrect Password. Attempt {currentAttempts} of 3."
                            End If
                        End If
                    Else
                        lblError.Text = "User does not exist."
                    End If
                End Using
            End Using

        Catch ex As Exception
            MsgBox("Login Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub ResetAttemptsAndLockout(user As String)
        Dim updateSql As String = "UPDATE users SET LoginAttempts=0, AccountStatus='Active' WHERE Username=@user"
        Using cmd As New MySqlCommand(updateSql, cn)
            cmd.Parameters.AddWithValue("@user", user)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub UpdateAttemptCount(user As String, attempts As Integer)
        Dim updateSql As String = "UPDATE users SET LoginAttempts=@attempts WHERE Username=@user"
        Using cmd As New MySqlCommand(updateSql, cn)
            cmd.Parameters.AddWithValue("@attempts", attempts)
            cmd.Parameters.AddWithValue("@user", user)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub LockoutAccount(user As String, attempts As Integer)
        Dim updateSql As String = "UPDATE users SET LoginAttempts=@attempts, AccountStatus='Locked' WHERE Username=@user"
        Using cmd As New MySqlCommand(updateSql, cn)
            cmd.Parameters.AddWithValue("@attempts", attempts)
            cmd.Parameters.AddWithValue("@user", user)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub Timer1_Tick_1(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        Me.Hide()
        frmMain.Show()
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