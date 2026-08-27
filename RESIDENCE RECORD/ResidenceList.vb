Imports MySql.Data.MySqlClient

Public Class ResidenceList

    ' Public properties to pass selected resident details back to the calling form
    Public Property SelectedResidentID As Integer = 0
    Public Property SelectedFullName As String = ""
    Public Property SelectedEmail As String = ""
    Public Property SelectedPhone As String = ""
    Public Property SelectedAddress As String = ""

    Private Sub ResidenceList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StyleDataGridView(dgvResidences)
        LoadResidences()
    End Sub

    ' --- 1. DATAGRIDVIEW CUSTOM DESIGN ---
    Private Sub StyleDataGridView(dgv As DataGridView)
        dgv.EnableHeadersVisualStyles = False
        dgv.BorderStyle = BorderStyle.None
        dgv.BackgroundColor = Color.White
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        dgv.GridColor = Color.FromArgb(220, 224, 230)
        dgv.RowHeadersVisible = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.MultiSelect = False
        dgv.AllowUserToResizeRows = False

        ' Header Styling
        Dim headerStyle As New DataGridViewCellStyle()
        headerStyle.BackColor = Color.FromArgb(248, 249, 252)
        headerStyle.ForeColor = Color.FromArgb(50, 50, 60)
        headerStyle.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
        headerStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        headerStyle.Padding = New Padding(10, 8, 10, 8)

        dgv.ColumnHeadersDefaultCellStyle = headerStyle
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        dgv.ColumnHeadersHeight = 40
        dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing

        ' Row Styling
        Dim defaultRowStyle As New DataGridViewCellStyle()
        defaultRowStyle.BackColor = Color.White
        defaultRowStyle.ForeColor = Color.FromArgb(50, 50, 60)
        defaultRowStyle.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular)
        defaultRowStyle.SelectionBackColor = Color.FromArgb(210, 215, 240)
        defaultRowStyle.SelectionForeColor = Color.Black
        defaultRowStyle.Padding = New Padding(10, 4, 10, 4)

        ' Alternating Soft Purple Rows
        Dim alternatingRowStyle As New DataGridViewCellStyle(defaultRowStyle)
        alternatingRowStyle.BackColor = Color.FromArgb(235, 237, 255)

        dgv.DefaultCellStyle = defaultRowStyle
        dgv.AlternatingRowsDefaultCellStyle = alternatingRowStyle
        dgv.RowTemplate.Height = 32
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    ' --- 2. LOAD RESIDENCES FROM DATABASE ---
    Private Sub LoadResidences()
        Try
            connection()

            ' Fetch key resident details matching your database schema
            sql = "SELECT ResidentID, ResidentCode AS 'Resident Code', FullName AS 'Full Name', " &
                  "MobileNumber AS 'Contact No.', Email, Address, AccountStatus AS 'Status' " &
                  "FROM residences " &
                  "ORDER BY ResidentID DESC"

            cmd = New MySqlCommand(sql, cn)
            Dim da As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            dgvResidences.DataSource = dt

            ' Optional: Hide primary key column if you don't want it visible to users
            If dgvResidences.Columns.Contains("ResidentID") Then
                dgvResidences.Columns("ResidentID").Visible = False
            End If

        Catch ex As Exception
            MsgBox("Error loading residences list: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            CloseConnection()
        End Try
    End Sub

    ' --- 3. LIVE SEARCH FILTER ---
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtName.TextChanged
        Dim filterText As String = txtName.Text.Trim().Replace("'", "''")
        If dgvResidences.DataSource IsNot Nothing Then
            Dim dt As DataTable = CType(dgvResidences.DataSource, DataTable)
            dt.DefaultView.RowFilter = $"`Resident Code` LIKE '%{filterText}%' OR `Full Name` LIKE '%{filterText}%' OR `Contact No.` LIKE '%{filterText}%' OR Address LIKE '%{filterText}%'"
        End If
    End Sub

    ' --- 4. INSERT BUTTON CLICK (SELECT & CLOSE) ---
    Private Sub btnInsert_Click(sender As Object, e As EventArgs) Handles btnInsert.Click
        ' Open the ResidenceList form as a dialog
        Using frmList As New ResidenceList()
            If frmList.ShowDialog() = DialogResult.OK Then
                ' Retrieve selected resident details from ResidenceList
                SelectedResidentID = frmList.SelectedResidentID
                SelectedFullName = frmList.SelectedFullName
                SelectedEmail = frmList.SelectedEmail
                SelectedPhone = frmList.SelectedPhone
                SelectedAddress = frmList.SelectedAddress

                ' Populate Name Textbox
                txtName.Text = SelectedFullName
                txtName.ReadOnly = True
            End If
        End Using
    End Sub

    ' --- 5. DOUBLE-CLICK ROW TO SELECT ---
    Private Sub dgvResidences_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvResidences.CellDoubleClick
        If e.RowIndex >= 0 Then
            SelectAndClose()
        End If
    End Sub

    Private Sub SelectAndClose()
        If dgvResidences.CurrentRow IsNot Nothing Then
            Dim row As DataGridViewRow = dgvResidences.CurrentRow

            SelectedResidentID = Convert.ToInt32(row.Cells("ResidentID").Value)
            SelectedFullName = row.Cells("Full Name").Value.ToString()
            SelectedEmail = If(IsDBNull(row.Cells("Email").Value), "", row.Cells("Email").Value.ToString())
            SelectedPhone = If(IsDBNull(row.Cells("Contact No.").Value), "", row.Cells("Contact No.").Value.ToString())
            SelectedAddress = If(IsDBNull(row.Cells("Address").Value), "", row.Cells("Address").Value.ToString())

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Else
            MsgBox("Please select a resident from the list first.", MsgBoxStyle.Exclamation)
        End If
    End Sub
End Class