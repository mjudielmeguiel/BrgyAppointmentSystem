Imports MySql.Data.MySqlClient

Public Class frmRecords

    Private Sub frmRecords_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadResidentsData()
    End Sub

    Public Sub LoadResidentsData(Optional searchKeyword As String = "")
        Try
            DBconnection.connection()

            Dim sql As String = "SELECT IDNumber, LastName, FirstName, Gender, DateOfBirth, Age, CivilStatus, Nationality, EmailAddress, PhoneNumber FROM residents"

            If Not String.IsNullOrWhiteSpace(searchKeyword) Then
                sql &= " WHERE LastName LIKE @Keyword OR FirstName LIKE @Keyword OR IDNumber LIKE @Keyword"
            End If

            sql &= " ORDER BY LastName ASC"

            Using cmd As New MySqlCommand(sql, DBconnection.cn)
                If Not String.IsNullOrWhiteSpace(searchKeyword) Then
                    cmd.Parameters.AddWithValue("@Keyword", "%" & searchKeyword.Trim() & "%")
                End If

                Using dt As New DataTable()
                    Using da As New MySqlDataAdapter(cmd)
                        dt.Clear()
                        da.Fill(dt)
                    End Using

                    dgvResidents.DataSource = dt
                    lblTotalRecords.Text = "Total Records: " & dt.Rows.Count.ToString()
                    FormatDataGridView()
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading records: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            DBconnection.CloseConnection()
        End Try
    End Sub

    Private Sub FormatDataGridView()
        With dgvResidents
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .AllowUserToAddRows = False
            .RowHeadersVisible = False
            .ReadOnly = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect

            If .Columns.Count > 0 Then
                .Columns("IDNumber").HeaderText = "Resident ID"
                .Columns("LastName").HeaderText = "Last Name"
                .Columns("FirstName").HeaderText = "First Name"
                .Columns("Gender").HeaderText = "Gender"
                .Columns("DateOfBirth").HeaderText = "Birth Date"
                .Columns("Age").HeaderText = "Age"
                .Columns("CivilStatus").HeaderText = "Civil Status"
                .Columns("Nationality").HeaderText = "Nationality"
                .Columns("EmailAddress").HeaderText = "Email Address"
                .Columns("PhoneNumber").HeaderText = "Phone Number"
            End If
        End With
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        LoadResidentsData(txtSearch.Text)
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        txtSearch.Clear()
        LoadResidentsData()
    End Sub

    Private Sub btnAddNewRecord_Click(sender As Object, e As EventArgs) Handles btnAddNewRecord.Click
        Dim frm As New frmBookAppointment()
        frm.ShowDialog()
        LoadResidentsData()
    End Sub

    Private Sub btnSetAppointment_Click(sender As Object, e As EventArgs) Handles btnSetAppointment.Click
        Dim frm As New frmBookAppointment()
        frm.ShowDialog()
        LoadResidentsData()
    End Sub

    Private Sub dgvResidents_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvResidents.CellDoubleClick
        If e.RowIndex >= 0 Then
            Dim idNumber As String = dgvResidents.Rows(e.RowIndex).Cells("IDNumber").Value.ToString()
            Dim frm As New frmBook()
            frm.LoadResidentData(idNumber)
            frm.ShowDialog()
        End If
    End Sub

End Class