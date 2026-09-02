Imports MySql.Data.MySqlClient

Public Class frmResidence_Records

    Private ReadOnly placeholderText As String = "Search Resident Name..."

    Private Sub frmResidence_Records_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblDateTime.Text = DateTime.Now.ToString("F")
        Timer1.Interval = 1000
        Timer1.Start()

        SetupSearchPlaceholder()
        LoadWelcomeUser()
        StyleDataGridView(dgvResidences)
        LoadResidenceRecords()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lblDateTime.Text = DateTime.Now.ToString("F")
    End Sub

    Private Sub LoadWelcomeUser()
        If Not String.IsNullOrEmpty(LoggedFullname) Then
            lblWelcomeUser.Text = $"{LoggedFullname}!"
        Else
            lblWelcomeUser.Text = "User!"
        End If
    End Sub

    Private Sub LoadResidenceRecords(Optional searchKeyword As String = "")
        Try
            connection()

            sql = "SELECT ResidentCode AS 'Resident Code', FullName AS 'Full Name', Gender, " &
                  "Birthday, MobileNumber AS 'Mobile No.', Email, CivilStatus AS 'Civil Status', " &
                  "Address, AccountStatus AS 'Status', CreatedAt AS 'Date Registered' " &
                  "FROM residences "

            If Not String.IsNullOrEmpty(searchKeyword) AndAlso searchKeyword <> placeholderText Then
                sql &= "WHERE FullName LIKE @search "
            End If

            sql &= "ORDER BY ResidentID DESC"

            cmd = New MySqlCommand(sql, cn)

            If Not String.IsNullOrEmpty(searchKeyword) AndAlso searchKeyword <> placeholderText Then
                cmd.Parameters.AddWithValue("@search", $"%{searchKeyword.Trim()}%")
            End If

            Dim da As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            dgvResidences.DataSource = dt
            lblTotalRecords.Text = $"Total Records: {dt.Rows.Count}"

        Catch ex As Exception
            MsgBox("Error loading residence records: " & ex.Message, MsgBoxStyle.Critical, "Database Error")
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub SetupSearchPlaceholder()
        If txtSearch IsNot Nothing Then
            txtSearch.Text = placeholderText
            txtSearch.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub txtSearch_Enter(sender As Object, e As EventArgs) Handles txtSearch.Enter
        If txtSearch.Text = placeholderText Then
            txtSearch.Text = ""
            txtSearch.ForeColor = Color.Black
        End If
    End Sub

    Private Sub txtSearch_Leave(sender As Object, e As EventArgs) Handles txtSearch.Leave
        If String.IsNullOrWhiteSpace(txtSearch.Text) Then
            SetupSearchPlaceholder()
            LoadResidenceRecords()
        End If
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If txtSearch.Text <> placeholderText Then
            LoadResidenceRecords(txtSearch.Text)
        End If
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        SetupSearchPlaceholder()
        LoadResidenceRecords()
    End Sub

    Private Sub StyleDataGridView(dgv As DataGridView)
        If dgv Is Nothing Then Return

        dgv.EnableHeadersVisualStyles = False
        dgv.BorderStyle = BorderStyle.None
        dgv.BackgroundColor = Color.White
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        dgv.GridColor = Color.FromArgb(220, 224, 230)
        dgv.RowHeadersVisible = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.MultiSelect = False
        dgv.AllowUserToResizeRows = False

        Dim headerStyle As New DataGridViewCellStyle()
        headerStyle.BackColor = Color.FromArgb(10, 25, 100)
        headerStyle.ForeColor = Color.White
        headerStyle.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
        headerStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        headerStyle.Padding = New Padding(8, 6, 8, 6)

        dgv.ColumnHeadersDefaultCellStyle = headerStyle
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        dgv.ColumnHeadersHeight = 38
        dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing

        Dim defaultRowStyle As New DataGridViewCellStyle()
        defaultRowStyle.BackColor = Color.White
        defaultRowStyle.ForeColor = Color.FromArgb(50, 50, 60)
        defaultRowStyle.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular)
        defaultRowStyle.SelectionBackColor = Color.FromArgb(210, 215, 240)
        defaultRowStyle.SelectionForeColor = Color.Black
        defaultRowStyle.Padding = New Padding(8, 4, 8, 4)

        Dim alternatingRowStyle As New DataGridViewCellStyle(defaultRowStyle)
        alternatingRowStyle.BackColor = Color.FromArgb(245, 247, 252)

        dgv.DefaultCellStyle = defaultRowStyle
        dgv.AlternatingRowsDefaultCellStyle = alternatingRowStyle
        dgv.RowTemplate.Height = 32
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    Private Sub btnCreateRequest_Click(sender As Object, e As EventArgs) Handles btnCreateRequest.Click
        Barangay_Residences.Show()
    End Sub

End Class