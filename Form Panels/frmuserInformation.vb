Imports MySql.Data.MySqlClient

Public Class frmuserInformation

    Private Sub frmuserInformation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadAllUsers()
    End Sub

    Private Sub LoadAllUsers()
        Dim adminDeptID As Integer = 0

        Try
            connection()

            sql = "SELECT AdminID FROM admin WHERE FullName=@adminname"
            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@adminname", LoggedFullname)
            dr = cmd.ExecuteReader()

            If dr.Read() Then
                adminDeptID = Convert.ToInt32(dr("AdminID"))
            End If
            dr.Close()

            sql = "SELECT UserID, Username, Firstname, Lastname, Role, AccountStatus, LoginAttempts, FullName " &
                  "FROM users WHERE DepartmentID=@deptid"

            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@deptid", adminDeptID)
            dr = cmd.ExecuteReader()

            Dim dtUsers As New DataTable()
            dtUsers.Load(dr)

            DataGridView1.DataSource = dtUsers

            DataGridView1.Columns("UserID").HeaderText = "ID"
            DataGridView1.Columns("Username").HeaderText = "Username"
            DataGridView1.Columns("Firstname").HeaderText = "First Name"
            DataGridView1.Columns("Lastname").HeaderText = "Last Name"
            DataGridView1.Columns("Role").HeaderText = "Role"
            DataGridView1.Columns("AccountStatus").HeaderText = "Account Status"
            DataGridView1.Columns("LoginAttempts").HeaderText = "Login Attempts"
            DataGridView1.Columns("FullName").Visible = False

            CloseConnection()
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnReset_Click_1(sender As Object, e As EventArgs) Handles btnReset.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("Please select a user first!", MsgBoxStyle.Exclamation)
            Return
        End If

        Dim selectedUserID As Integer = Convert.ToInt32(DataGridView1.SelectedRows(0).Cells("UserID").Value)
        Dim currentStatus As String = DataGridView1.SelectedRows(0).Cells("AccountStatus").Value.ToString()

        If currentStatus <> "Locked" Then
            MsgBox("Only Locked accounts can be reset!", MsgBoxStyle.Information)
            Return
        End If

        Try
            connection()

            sql = "UPDATE users SET AccountStatus = 'Active', LoginAttempts = 0 WHERE UserID = @uid"
            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@uid", selectedUserID)
            cmd.ExecuteNonQuery()

            MsgBox("Account Reset Successfully!", MsgBoxStyle.Information)
            LoadAllUsers()

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub BtnDelete_Click_1(sender As Object, e As EventArgs) Handles BtnDelete.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("Please select a user first!", MsgBoxStyle.Exclamation)
            Return
        End If

        Dim selectedFullName As String = DataGridView1.SelectedRows(0).Cells("FullName").Value.ToString()
        Dim selectedUserID As Integer = Convert.ToInt32(DataGridView1.SelectedRows(0).Cells("UserID").Value)
        Dim selectedUser As String = DataGridView1.SelectedRows(0).Cells("Username").Value.ToString()

        If selectedFullName = LoggedFullname Then
            MsgBox("You cannot delete your own account!", MsgBoxStyle.Exclamation)
            Return
        End If

        If MsgBox($"Are you sure you want to delete user: {selectedUser}?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Confirm Delete") = MsgBoxResult.No Then
            Return
        End If

        Try
            connection()

            sql = "DELETE FROM users WHERE UserID = @uid"
            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@uid", selectedUserID)
            cmd.ExecuteNonQuery()

            MsgBox("User deleted successfully!", MsgBoxStyle.Information)
            LoadAllUsers()

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub btnnewadmin_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick


    End Sub
End Class