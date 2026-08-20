Imports MySql.Data.MySqlClient

Public Class frmChangePassword

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If txtCurrentPass.Text = "" Or txtNewPass.Text = "" Or txtConfirmNewPass.Text = "" Then
            MsgBox("Please fill all fields!", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If txtNewPass.Text <> txtConfirmNewPass.Text Then
            MsgBox("New Password does not match!", MsgBoxStyle.Exclamation)
            txtNewPass.Clear()
            txtConfirmNewPass.Clear()
            txtNewPass.Focus()
            Exit Sub
        End If

        If txtNewPass.Text.Trim() = txtCurrentPass.Text.Trim() Then
            MsgBox("New Password cannot be the same as your Current Password!", MsgBoxStyle.Exclamation)
            txtNewPass.Clear()
            txtConfirmNewPass.Clear()
            txtNewPass.Focus()
            Exit Sub
        End If

        Call connection()

        Try

            sql = "SELECT Password FROM users WHERE FullName=@name AND Password=@current"
            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@name", DBconnection.LoggedFullname)
            cmd.Parameters.AddWithValue("@current", txtCurrentPass.Text)
            dr = cmd.ExecuteReader()

            If Not dr.HasRows Then
                MsgBox("Current Password is incorrect!", MsgBoxStyle.Exclamation)
                dr.Close()
                Call DBconnection.CloseConnection()
                ClearAllFields()
                Exit Sub
            End If
            dr.Close()

            sql = "UPDATE users SET Password=@newpass WHERE FullName=@name"
            cmd = New MySqlCommand(sql, cn)
            With cmd
                .Parameters.AddWithValue("@newpass", txtNewPass.Text.Trim())
                .Parameters.AddWithValue("@name", DBconnection.LoggedFullname)
                .ExecuteNonQuery()
            End With

            MsgBox("Password changed successfully!", MsgBoxStyle.Information)
            Call DBconnection.CloseConnection()
            ClearAllFields()

            Me.Hide()
            Dim main As New frmMain
            main.Show()

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
            Call DBconnection.CloseConnection()
            ClearAllFields()
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Hide()
        Dim main As New frmMain

        Me.TopMost = False
        main.Enabled = True
        main.TopMost = True
        main.Show()
    End Sub

    Private Sub ClearAllFields()
        txtCurrentPass.Clear()
        txtNewPass.Clear()
        txtConfirmNewPass.Clear()
        lblPassStatus.Text = ""
        txtCurrentPass.Focus()
    End Sub

    Private Sub CheckPasswordMatch()
        If String.IsNullOrWhiteSpace(txtNewPass.Text) AndAlso String.IsNullOrWhiteSpace(txtConfirmNewPass.Text) Then
            lblPassStatus.Text = ""
            lblPassStatus.ForeColor = Color.Black
            Return
        End If

        If txtNewPass.Text.Trim() = txtConfirmNewPass.Text.Trim() Then
            lblPassStatus.Text = "✓ Password Match"
            lblPassStatus.ForeColor = Color.Green
        Else
            lblPassStatus.Text = "✗ Password does not match"
            lblPassStatus.ForeColor = Color.Red
        End If
    End Sub

    Private Sub txtNewPass_TextChanged(sender As Object, e As EventArgs) Handles txtNewPass.TextChanged, txtConfirmNewPass.TextChanged
        CheckPasswordMatch()
    End Sub

    Private Sub frmChangePassword_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.TopMost = True
        frmMain.Enabled = False
        lblPassStatus.Text = ""
    End Sub
End Class