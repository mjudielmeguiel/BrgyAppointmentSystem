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

    Private Sub checkifuserexist()
        connection()
        lblError.Text = ""
        lblAttempts.Text = "0"

        sql = "SELECT LoginAttempts, AccountStatus FROM users WHERE Username=@user"
        cmd = New MySqlCommand(sql, cn)
        cmd.Parameters.AddWithValue("@user", txtUsername.Text.Trim())
        dr = cmd.ExecuteReader()

        If dr.Read() Then
            lblAttempts.Text = If(IsDBNull(dr("LoginAttempts")), 0, dr("LoginAttempts")).ToString()

            If dr("AccountStatus").ToString().Trim() = "Locked" Then
                lblError.Text = "Your account is locked/deactivated."
                dr.Close()
                CloseConnection()
                Exit Sub
            End If

            dr.Close()
            CloseConnection()
            Login()
        Else
            dr.Close()
            CloseConnection()

            connection()
            sql = "SELECT AdminID FROM admin WHERE Username=@user"
            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@user", txtUsername.Text.Trim())
            dr = cmd.ExecuteReader()

            If dr.Read() Then
                dr.Close()
                CloseConnection()
                LoginAdmin()
            Else
                lblError.Text = "User does not exist."
                dr.Close()
                CloseConnection()
            End If
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Application.Exit()
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

        checkifuserexist()
    End Sub

    Private Sub Login()
        connection()
        lblError.Text = ""

        sql = "SELECT * FROM users WHERE Username=@user AND Password=@pass"
        cmd = New MySqlCommand(sql, cn)
        cmd.Parameters.AddWithValue("@user", txtUsername.Text.Trim())
        cmd.Parameters.AddWithValue("@pass", txtPassword.Text)
        dr = cmd.ExecuteReader()

        If dr.Read() Then
            LoggedFullname = dr("FullName").ToString()
            LoggedRole = dr("Role").ToString().Trim()
            Dim currentUser As String = txtUsername.Text.Trim()
            dr.Close()

            sql = "UPDATE users SET LoginAttempts=0, AccountStatus='Active' WHERE Username=@user"
            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@user", currentUser)
            cmd.ExecuteNonQuery()

            CloseConnection()

            lblError.Text = "✓ Login Success! Redirecting..."
            Timer1.Interval = 800
            Timer1.Start()
        Else
            dr.Close()
            Dim currentAttempts As Integer
            If Not Integer.TryParse(lblAttempts.Text, currentAttempts) Then currentAttempts = 0
            currentAttempts += 1
            lblAttempts.Text = currentAttempts.ToString()

            UpdateAttempts()

            If currentAttempts >= 3 Then
                lblError.Text = "3 failed attempts reached. Account locked."
                DeactAccts()
            Else
                lblError.Text = $"✗ Incorrect Password. Attempt {currentAttempts} of 3."
            End If

            CloseConnection()
        End If
    End Sub

    Private Sub LoginAdmin()
        connection()
        lblError.Text = ""

        sql = "SELECT * FROM admin WHERE Username=@user AND Password=@pass"
        cmd = New MySqlCommand(sql, cn)
        cmd.Parameters.AddWithValue("@user", txtUsername.Text.Trim())
        cmd.Parameters.AddWithValue("@pass", txtPassword.Text)
        dr = cmd.ExecuteReader()

        If dr.Read() Then
            LoggedFullname = dr("FullName").ToString()
            LoggedRole = "Administrator"
            dr.Close()
            CloseConnection()

            lblError.Text = "✓ Admin Login Success! Redirecting..."
            Timer1.Interval = 800
            Timer1.Start()
        Else
            dr.Close()
            lblError.Text = "✗ Incorrect Admin Password."
            CloseConnection()
        End If
    End Sub

    Private Sub Timer1_Tick_1(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        Me.Hide()
        frmMain.Show()
    End Sub

    Private Sub UpdateAttempts()
        connection()
        sql = "UPDATE users SET LoginAttempts=@attempts WHERE Username=@user"
        cmd = New MySqlCommand(sql, cn)
        cmd.Parameters.AddWithValue("@attempts", lblAttempts.Text)
        cmd.Parameters.AddWithValue("@user", txtUsername.Text.Trim())
        cmd.ExecuteNonQuery()
        CloseConnection()
    End Sub

    Private Sub DeactAccts()
        connection()
        sql = "UPDATE users SET AccountStatus='Locked' WHERE Username=@user"
        cmd = New MySqlCommand(sql, cn)
        cmd.Parameters.AddWithValue("@user", txtUsername.Text.Trim())
        cmd.ExecuteNonQuery()
        CloseConnection()
    End Sub

    Private Sub btnShowPass_Click(sender As Object, e As EventArgs) Handles btnShowPass.Click
        txtPassword.PasswordChar = If(txtPassword.PasswordChar = "●"c, Char.MinValue, "●"c)
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Me.Hide()
        frmcreateadmin.Show()
    End Sub

End Class