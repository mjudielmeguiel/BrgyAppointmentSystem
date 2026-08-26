Imports MySql.Data.MySqlClient

Public Class frmManage_Users

    Private Sub frmManage_Users_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadAllUsers()
        UpdateUserCounts()
    End Sub

    Private Sub LoadAllUsers(Optional ByVal searchQuery As String = "")
        Dim adminDeptID As Integer = 0

        Try
            connection()

            ' Get logged-in Admin's Department ID
            Dim adminSql As String = "SELECT AdminID FROM admin WHERE FullName=@adminname"
            Using cmdAdmin As New MySqlCommand(adminSql, cn)
                cmdAdmin.Parameters.AddWithValue("@adminname", LoggedFullname)
                Using drAdmin As MySqlDataReader = cmdAdmin.ExecuteReader()
                    If drAdmin.Read() Then
                        adminDeptID = Convert.ToInt32(drAdmin("AdminID"))
                    End If
                End Using
            End Using

            ' Select comprehensive basic information from users table
            Dim querySql As String = ""
            If String.IsNullOrWhiteSpace(searchQuery) Then
                querySql = "SELECT UserID, StaffCode, FullName, Role, Email, MobileNumber, AccountStatus, " &
                           "Birthday, Gender, CivilStatus, LoginAttempts " &
                           "FROM users WHERE DepartmentID=@deptid"
            Else
                querySql = "SELECT UserID, StaffCode, FullName, Role, Email, MobileNumber, AccountStatus, " &
                           "Birthday, Gender, CivilStatus, LoginAttempts " &
                           "FROM users WHERE DepartmentID=@deptid AND " &
                           "(Username LIKE @search OR Firstname LIKE @search OR Lastname LIKE @search OR " &
                           "FullName LIKE @search OR Role LIKE @search OR StaffCode LIKE @search OR Email LIKE @search OR MobileNumber LIKE @search)"
            End If

            Using cmdUsers As New MySqlCommand(querySql, cn)
                cmdUsers.Parameters.AddWithValue("@deptid", adminDeptID)
                If Not String.IsNullOrWhiteSpace(searchQuery) Then
                    cmdUsers.Parameters.AddWithValue("@search", "%" & searchQuery & "%")
                End If

                Using drUsers As MySqlDataReader = cmdUsers.ExecuteReader()
                    Dim dtUsers As New DataTable()
                    dtUsers.Load(drUsers)
                    DataGridView1.DataSource = dtUsers
                End Using
            End Using

            ' Format DataGridView Headers for clean visualization
            If DataGridView1.Columns.Contains("UserID") Then DataGridView1.Columns("UserID").HeaderText = "User ID"
            If DataGridView1.Columns.Contains("StaffCode") Then DataGridView1.Columns("StaffCode").HeaderText = "Staff Code"
            If DataGridView1.Columns.Contains("FullName") Then DataGridView1.Columns("FullName").HeaderText = "Full Name"
            If DataGridView1.Columns.Contains("Role") Then DataGridView1.Columns("Role").HeaderText = "Role / Position"
            If DataGridView1.Columns.Contains("Email") Then DataGridView1.Columns("Email").HeaderText = "Email Address"
            If DataGridView1.Columns.Contains("MobileNumber") Then DataGridView1.Columns("MobileNumber").HeaderText = "Mobile No."
            If DataGridView1.Columns.Contains("AccountStatus") Then DataGridView1.Columns("AccountStatus").HeaderText = "Status"
            If DataGridView1.Columns.Contains("Birthday") Then DataGridView1.Columns("Birthday").HeaderText = "Birth Date"
            If DataGridView1.Columns.Contains("Gender") Then DataGridView1.Columns("Gender").HeaderText = "Gender"
            If DataGridView1.Columns.Contains("CivilStatus") Then DataGridView1.Columns("CivilStatus").HeaderText = "Civil Status"
            If DataGridView1.Columns.Contains("LoginAttempts") Then DataGridView1.Columns("LoginAttempts").HeaderText = "Failed Attempts"

            UpdateUserCounts()

        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub UpdateUserCounts()
        Dim adminDeptID As Integer = 0
        Dim totalCount As Integer = 0
        Dim activeCount As Integer = 0
        Dim lockedCount As Integer = 0

        Try
            If cn.State <> ConnectionState.Open Then
                connection()
            End If

            Dim adminSql As String = "SELECT AdminID FROM admin WHERE FullName=@adminname"
            Using cmdAdmin As New MySqlCommand(adminSql, cn)
                cmdAdmin.Parameters.AddWithValue("@adminname", LoggedFullname)
                Using drAdmin As MySqlDataReader = cmdAdmin.ExecuteReader()
                    If drAdmin.Read() Then
                        adminDeptID = Convert.ToInt32(drAdmin("AdminID"))
                    End If
                End Using
            End Using

            Using cmdTotal As New MySqlCommand("SELECT COUNT(*) FROM users WHERE DepartmentID=@deptid", cn)
                cmdTotal.Parameters.AddWithValue("@deptid", adminDeptID)
                totalCount = Convert.ToInt32(cmdTotal.ExecuteScalar())
            End Using

            Using cmdActive As New MySqlCommand("SELECT COUNT(*) FROM users WHERE DepartmentID=@deptid AND AccountStatus='Active'", cn)
                cmdActive.Parameters.AddWithValue("@deptid", adminDeptID)
                activeCount = Convert.ToInt32(cmdActive.ExecuteScalar())
            End Using

            Using cmdLocked As New MySqlCommand("SELECT COUNT(*) FROM users WHERE DepartmentID=@deptid AND AccountStatus='Locked'", cn)
                cmdLocked.Parameters.AddWithValue("@deptid", adminDeptID)
                lockedCount = Convert.ToInt32(cmdLocked.ExecuteScalar())
            End Using

            lblTotalUsers.Text = "Total Users: " & totalCount
            lblActiveUsers.Text = "Active Users: " & activeCount
            lblLockedUsers.Text = "Locked: " & lockedCount

        Catch ex As Exception
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        LoadAllUsers(txtSearch.Text.Trim())
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        txtSearch.Clear()
        LoadAllUsers()
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If e.RowIndex < 0 Then Exit Sub

        Dim selectedUserID As Integer = Convert.ToInt32(DataGridView1.Rows(e.RowIndex).Cells("UserID").Value)
        Dim frm As New frmUserInformation(selectedUserID)
        If frm.ShowDialog() = DialogResult.OK Then
            LoadAllUsers()
        End If
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

            Dim updateSql As String = "UPDATE users SET AccountStatus = 'Active', LoginAttempts = 0 WHERE UserID = @uid"
            Using cmdUpdate As New MySqlCommand(updateSql, cn)
                cmdUpdate.Parameters.AddWithValue("@uid", selectedUserID)
                cmdUpdate.ExecuteNonQuery()
            End Using

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

        If selectedFullName = LoggedFullname Then
            MsgBox("You cannot delete your own account!", MsgBoxStyle.Exclamation)
            Return
        End If

        If MsgBox($"Are you sure you want to delete user: {selectedFullName}?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Confirm Delete") = MsgBoxResult.No Then
            Return
        End If

        Try
            connection()

            Dim deleteSql As String = "DELETE FROM users WHERE UserID = @uid"
            Using cmdDelete As New MySqlCommand(deleteSql, cn)
                cmdDelete.Parameters.AddWithValue("@uid", selectedUserID)
                cmdDelete.ExecuteNonQuery()
            End Using

            MsgBox("User deleted successfully!", MsgBoxStyle.Information)
            LoadAllUsers()

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            CloseConnection()
        End Try
    End Sub

End Class