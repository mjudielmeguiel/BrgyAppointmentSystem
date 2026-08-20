Imports MySql.Data.MySqlClient

Public Class frmcreateadmin

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If txtDepartment.Text = "" Or txtLastname.Text = "" Or txtFirstname.Text = "" Or txtUsername.Text = "" Or txtPassword.Text = "" Or txtConfirmPass.Text = "" Then
            MsgBox("Please fill all fields including Department!", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If txtPassword.Text <> txtConfirmPass.Text Then
            Exit Sub
        End If

        Call connection()

        Try
            sql = "SELECT AdminID FROM admin"
            cmd = New MySqlCommand(sql, cn)
            dr = cmd.ExecuteReader()

            If dr.HasRows Then
                MsgBox("Admin account already exists! Only one Admin is allowed.", MsgBoxStyle.Exclamation)
                dr.Close()
                Call DBconnection.CloseConnection()
                ClearAllFields()
                Exit Sub
            End If
            dr.Close()

            Dim fullname As String = Trim(txtLastname.Text) & ", " & Trim(txtFirstname.Text)
            sql = "SELECT FullName FROM admin WHERE FullName=@full"
            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@full", fullname)
            dr = cmd.ExecuteReader()

            If dr.HasRows Then
                MsgBox("Full Name already exists!", MsgBoxStyle.Exclamation)
                dr.Close()
                Call DBconnection.CloseConnection()
                ClearAllFields()
                Exit Sub
            End If
            dr.Close()

            sql = "SELECT Username FROM admin WHERE Username=@user"
            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@user", txtUsername.Text)
            dr = cmd.ExecuteReader()

            If dr.HasRows Then
                MsgBox("Username already exists!", MsgBoxStyle.Exclamation)
                dr.Close()
                Call DBconnection.CloseConnection()
                ClearAllFields()
                Exit Sub
            End If
            dr.Close()

            sql = "INSERT INTO admin (Department, Lastname, Firstname, FullName, Username, Password) 
                   VALUES (@dept, @last, @first, @full, @user, @pass)"

            cmd = New MySqlCommand(sql, cn)
            With cmd
                .Parameters.AddWithValue("@dept", txtDepartment.Text)
                .Parameters.AddWithValue("@last", txtLastname.Text)
                .Parameters.AddWithValue("@first", txtFirstname.Text)
                .Parameters.AddWithValue("@full", fullname)
                .Parameters.AddWithValue("@user", txtUsername.Text)
                .Parameters.AddWithValue("@pass", txtPassword.Text)
                .ExecuteNonQuery()
            End With

            MsgBox("Admin Account Created Successfully!", MsgBoxStyle.Information)
            Call DBconnection.CloseConnection()
            ClearAllFields()

            Me.Hide()
            Dim login As New frmlogin
            login.Show()

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
            Call DBconnection.CloseConnection()
            ClearAllFields()
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Hide()
        Dim login As New frmlogin
        login.Show()
    End Sub

    Private Sub ClearAllFields()
        txtDepartment.Clear()
        txtLastname.Clear()
        txtFirstname.Clear()
        txtUsername.Clear()
        txtPassword.Clear()
        txtConfirmPass.Clear()
        lblPassStatus.Text = ""
        txtDepartment.Focus()
    End Sub

    Private Sub CheckPasswordMatch()
        If String.IsNullOrWhiteSpace(txtPassword.Text) AndAlso String.IsNullOrWhiteSpace(txtConfirmPass.Text) Then
            lblPassStatus.Text = ""
            lblPassStatus.ForeColor = Color.Black
            Return
        End If

        If txtPassword.Text.Trim() = txtConfirmPass.Text.Trim() Then
            lblPassStatus.Text = "✓ Password Match"
            lblPassStatus.ForeColor = Color.Green
        Else
            lblPassStatus.Text = "✗ Password does not match"
            lblPassStatus.ForeColor = Color.Red
        End If
    End Sub

    Private Sub txtPassword_TextChanged(sender As Object, e As EventArgs) Handles txtPassword.TextChanged, txtConfirmPass.TextChanged
        CheckPasswordMatch()
    End Sub

    Private Sub frmcreateadmin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblPassStatus.Text = ""
    End Sub
End Class