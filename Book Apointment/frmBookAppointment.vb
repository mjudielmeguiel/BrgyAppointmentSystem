Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Drawing.Imaging
Imports MySql.Data.MySqlClient

Public Class frmBookAppointment

    ' ==================================================
    ' ✅ LAHAT NG TEXTBOX — AWTOMATIKONG CAPSLOCK
    ' ==================================================
    Private Sub txt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles _
        txtLastname.KeyPress, txtFirstname.KeyPress, txtNationality.KeyPress,
        txtBarangay.KeyPress, txtMunicipality.KeyPress, txtAddress.KeyPress

        ' ✅ GAWING MALAKING TITIK ANG LAHAT NG TINITYPE
        e.KeyChar = Char.ToUpper(e.KeyChar)
    End Sub

    Private Sub frmBookAppointment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmbGender.Items.Clear()
        cmbGender.Items.AddRange({"MALE", "FEMALE"})

        cmbCivilStatus.Items.Clear()
        cmbCivilStatus.Items.AddRange({"SINGLE", "MARRIED", "WIDOWED", "SEPARATED"})

        cmbSuffix.Items.Clear()
        cmbSuffix.Items.AddRange({"", "JR.", "SR.", "II", "III", "IV", "N/A"})

        dtpBirthDate.MaxDate = Date.Today
        dtpBirthDate.Value = Date.Today

        txtAge.ReadOnly = True
        lblIDNumber.Text = "ID: WILL BE GENERATED"

        ' ✅ AWTOMATIKONG LAGYAN NG BARANGAY, MUNICIPALITY, NATIONALITY AT EMAIL
        txtBarangay.Text = "PUTATAN"
        txtMunicipality.Text = "MUNTINLUPA CITY"
        txtNationality.Text = "FILIPINO"
        txtEmail.Text = "@GMAIL.COM"

        ' ✅ KULAY NG ERROR MESSAGE — DARK RED (hindi masakit sa mata)
        lblLastNameError.ForeColor = Color.FromArgb(160, 0, 0)
        lblFirstNameError.ForeColor = Color.FromArgb(160, 0, 0)
        lblGenderError.ForeColor = Color.FromArgb(160, 0, 0)
        lblBirthDateError.ForeColor = Color.FromArgb(160, 0, 0)
        lblCivilStatusError.ForeColor = Color.FromArgb(160, 0, 0)
        lblNationalityError.ForeColor = Color.FromArgb(160, 0, 0)
        lblEmailError.ForeColor = Color.FromArgb(160, 0, 0)
        lblPhoneError.ForeColor = Color.FromArgb(160, 0, 0)
        lblBarangayError.ForeColor = Color.FromArgb(160, 0, 0)
        lblMunicipalityError.ForeColor = Color.FromArgb(160, 0, 0)
        lblAddressError.ForeColor = Color.FromArgb(160, 0, 0)

        ClearValidation()
        CalculateAge()
    End Sub

    Private Sub dtpBirthDate_ValueChanged(sender As Object, e As EventArgs) Handles dtpBirthDate.ValueChanged
        CalculateAge()
        If dtpBirthDate.Value.Date <= Date.Today Then
            lblBirthDateError.Text = ""
        End If
    End Sub

    Private Sub CalculateAge()
        Dim birthDate As Date = dtpBirthDate.Value.Date
        Dim today As Date = Date.Today
        Dim age As Integer = today.Year - birthDate.Year
        If birthDate > today.AddYears(-age) Then age -= 1
        txtAge.Text = Math.Max(age, 0).ToString()
    End Sub

    Private Sub ClearValidation()
        lblLastNameError.Text = ""
        lblFirstNameError.Text = ""
        lblGenderError.Text = ""
        lblBirthDateError.Text = ""
        lblCivilStatusError.Text = ""
        lblNationalityError.Text = ""
        lblEmailError.Text = ""
        lblPhoneError.Text = ""
        lblBarangayError.Text = ""
        lblMunicipalityError.Text = ""
        lblAddressError.Text = ""
    End Sub

    ' ==================================================
    ' ✅ TIGNAN KUNG MAY KATULAD NA EMAIL SA DATABASE
    ' ==================================================
    Private Function IsEmailExists(email As String) As Boolean
        Try
            DBconnection.connection()
            Using cmd As New MySqlCommand("SELECT COUNT(*) FROM residents WHERE EmailAddress = @Email", DBconnection.cn)
                cmd.Parameters.AddWithValue("@Email", email.Trim())
                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                Return count > 0
            End Using
        Catch ex As Exception
            Return False
        Finally
            DBconnection.CloseConnection()
        End Try
    End Function

    ' ==================================================
    ' ✅ TIGNAN KUNG MAY KATULAD NA PHONE SA DATABASE
    ' ==================================================
    Private Function IsPhoneExists(phone As String) As Boolean
        Try
            DBconnection.connection()
            Using cmd As New MySqlCommand("SELECT COUNT(*) FROM residents WHERE PhoneNumber = @Phone", DBconnection.cn)
                cmd.Parameters.AddWithValue("@Phone", phone.Trim())
                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                Return count > 0
            End Using
        Catch ex As Exception
            Return False
        Finally
            DBconnection.CloseConnection()
        End Try
    End Function

    ' ==================================================
    ' ✅ VALIDATION — LAHAT NG FIELD MAY CHECK
    ' ==================================================
    Private Function ValidateForm() As Boolean
        Dim valid As Boolean = True
        ClearValidation()

        ' ✅ LAST NAME — Kailangan may laman
        If String.IsNullOrWhiteSpace(txtLastname.Text) Then
            lblLastNameError.Text = "LAST NAME IS REQUIRED. PLEASE ENTER A VALUE."
            valid = False
        End If

        ' ✅ FIRST NAME — Kailangan may laman
        If String.IsNullOrWhiteSpace(txtFirstname.Text) Then
            lblFirstNameError.Text = "FIRST NAME IS REQUIRED. PLEASE ENTER A VALUE."
            valid = False
        End If

        ' ✅ GENDER — Kailangan may napili
        If cmbGender.SelectedIndex = -1 Then
            lblGenderError.Text = "GENDER IS REQUIRED. PLEASE SELECT ONE."
            valid = False
        End If

        ' ✅ BIRTH DATE — Hindi pwede sa hinaharap
        If dtpBirthDate.Value.Date > Date.Today Then
            lblBirthDateError.Text = "BIRTH DATE IS INVALID. CANNOT BE A FUTURE DATE."
            valid = False
        End If

        ' ✅ CIVIL STATUS — Kailangan may napili
        If cmbCivilStatus.SelectedIndex = -1 Then
            lblCivilStatusError.Text = "CIVIL STATUS IS REQUIRED. PLEASE SELECT ONE."
            valid = False
        End If

        ' ✅ NATIONALITY — Kailangan may laman
        If String.IsNullOrWhiteSpace(txtNationality.Text) Then
            lblNationalityError.Text = "NATIONALITY IS REQUIRED. PLEASE ENTER A VALUE."
            valid = False
        End If

        ' ✅ BARANGAY — Kailangan may laman
        If String.IsNullOrWhiteSpace(txtBarangay.Text) Then
            lblBarangayError.Text = "BARANGAY IS REQUIRED. PLEASE ENTER A VALUE."
            valid = False
        End If

        ' ✅ MUNICIPALITY — Kailangan may laman
        If String.IsNullOrWhiteSpace(txtMunicipality.Text) Then
            lblMunicipalityError.Text = "MUNICIPALITY IS REQUIRED. PLEASE ENTER A VALUE."
            valid = False
        End If

        Dim email As String = txtEmail.Text.Trim()
        Dim phone As String = txtPhone.Text.Trim()

        ' ==================================================
        ' ✅ EMAIL — REQUIRED + TAMA ANG FORMAT + WALANG DOBLE
        ' ==================================================
        If String.IsNullOrWhiteSpace(email) Then
            lblEmailError.Text = "EMAIL IS REQUIRED. PLEASE ENTER A VALID EMAIL ADDRESS."
            valid = False
        ElseIf Not Regex.IsMatch(email, "^[^@\s]+@[^@\s]+\.[^@\s]+$") Then
            lblEmailError.Text = "INVALID EMAIL FORMAT. EXAMPLE: NAME@GMAIL.COM"
            valid = False
        ElseIf IsEmailExists(email) Then
            lblEmailError.Text = "EMAIL ALREADY REGISTERED. USE A DIFFERENT EMAIL."
            valid = False
        End If

        ' ✅ PHONE NUMBER — REQUIRED + 11 DIGITS + WALANG DOBLE
        If String.IsNullOrWhiteSpace(phone) Then
            lblPhoneError.Text = "CONTACT NUMBER IS REQUIRED. PLEASE ENTER A VALUE."
            valid = False
        ElseIf Not Regex.IsMatch(phone, "^[0-9]{11}$") Then
            lblPhoneError.Text = "CONTACT NUMBER MUST BE EXACTLY 11 DIGITS."
            valid = False
        ElseIf IsPhoneExists(phone) Then
            lblPhoneError.Text = "CONTACT NUMBER ALREADY REGISTERED. USE A DIFFERENT NUMBER."
            valid = False
        End If

        ' ✅ HINDI PWEDENG MAGKATULAD ANG EMAIL AT PHONE
        If email = phone Then
            lblEmailError.Text = "EMAIL AND CONTACT NUMBER CANNOT BE THE SAME."
            lblPhoneError.Text = "EMAIL AND CONTACT NUMBER CANNOT BE THE SAME."
            valid = False
        End If

        ' ✅ ADDRESS — Kailangan may laman
        If String.IsNullOrWhiteSpace(txtAddress.Text) Then
            lblAddressError.Text = "ADDRESS IS REQUIRED. PLEASE ENTER A VALUE."
            valid = False
        End If

        Return valid
    End Function

    ' ==================================================
    ' ✅ SAVE BUTTON — AWTOMATIKONG PAGSASAMA NG ADDRESS
    ' ==================================================
    Private Sub btnSetAppointment_Click(sender As Object, e As EventArgs) Handles btnSetAppointment.Click
        If Not ValidateForm() Then
            Exit Sub
        End If

        CalculateAge()
        Dim age As Integer = Integer.Parse(txtAge.Text)
        Dim newID As Integer = 0
        Dim generatedID As String = ""

        ' ✅ BUUIN ANG BUONG ADDRESS — [ADDRESS], BARANGAY, MUNICIPALITY
        Dim fullAddress As String = txtAddress.Text.Trim() & ", " & txtBarangay.Text.Trim() & ", " & txtMunicipality.Text.Trim()

        Try
            DBconnection.connection()

            Using transaction As MySqlTransaction = DBconnection.cn.BeginTransaction()
                Try
                    Dim temporaryID As String = "TEMP-" & Guid.NewGuid().ToString("N")

                    Dim insertSQL As String = "
                        INSERT INTO residents 
                        (IDNumber, LastName, FirstName, MI, Suffix, Gender, DateOfBirth, Age, 
                         CivilStatus, Nationality, EmailAddress, PhoneNumber, Address, Photo)
                        VALUES 
                        (@IDNumber, @LastName, @FirstName, @MI, @Suffix, @Gender, @DateOfBirth, @Age,
                         @CivilStatus, @Nationality, @EmailAddress, @PhoneNumber, @Address, @Photo);
                        SELECT LAST_INSERT_ID();
                    "

                    Using cmd As New MySqlCommand(insertSQL, DBconnection.cn, transaction)
                        cmd.Parameters.AddWithValue("@IDNumber", temporaryID)
                        cmd.Parameters.AddWithValue("@LastName", txtLastname.Text.Trim())
                        cmd.Parameters.AddWithValue("@FirstName", txtFirstname.Text.Trim())
                        cmd.Parameters.AddWithValue("@MI", If(String.IsNullOrWhiteSpace(txtMI.Text), DBNull.Value, txtMI.Text.Trim()))

                        ' ✅ SUFFIX — Kung walang napili → AWTOMATIKONG "N/A"
                        Dim suffixValue As String = cmbSuffix.Text.Trim().ToUpper()
                        If String.IsNullOrWhiteSpace(suffixValue) Then
                            suffixValue = "N/A"
                        End If
                        cmd.Parameters.AddWithValue("@Suffix", suffixValue)

                        cmd.Parameters.AddWithValue("@Gender", cmbGender.Text)
                        cmd.Parameters.AddWithValue("@DateOfBirth", dtpBirthDate.Value.Date)
                        cmd.Parameters.AddWithValue("@Age", age)
                        cmd.Parameters.AddWithValue("@CivilStatus", cmbCivilStatus.Text)
                        cmd.Parameters.AddWithValue("@Nationality", txtNationality.Text.Trim())
                        cmd.Parameters.AddWithValue("@EmailAddress", txtEmail.Text.Trim().ToUpper())
                        cmd.Parameters.AddWithValue("@PhoneNumber", txtPhone.Text.Trim())

                        ' ✅ I-SAVE ANG BUONG ADDRESS SA DATABASE
                        cmd.Parameters.AddWithValue("@Address", fullAddress)

                        ' ✅ PHOTO
                        If picResident.Image IsNot Nothing Then
                            Using ms As New MemoryStream()
                                picResident.Image.Save(ms, ImageFormat.Jpeg)
                                cmd.Parameters.AddWithValue("@Photo", ms.ToArray())
                            End Using
                        Else
                            cmd.Parameters.AddWithValue("@Photo", DBNull.Value)
                        End If

                        newID = Convert.ToInt32(cmd.ExecuteScalar())
                    End Using

                    ' ✅ GENERATE FINAL ID — PTR-XXXXXX
                    generatedID = "PTR-" & newID.ToString("D6")

                    ' ✅ UPDATE ANG FINAL ID SA DATABASE
                    Dim updateSQL As String = "UPDATE residents SET IDNumber = @IDNumber WHERE id = @ResidentID"
                    Using updateCmd As New MySqlCommand(updateSQL, DBconnection.cn, transaction)
                        updateCmd.Parameters.AddWithValue("@IDNumber", generatedID)
                        updateCmd.Parameters.AddWithValue("@ResidentID", newID)
                        updateCmd.ExecuteNonQuery()
                    End Using

                    transaction.Commit()
                Catch ex As Exception
                    transaction.Rollback()
                    Throw ex
                End Try
            End Using

            ' ✅ SUCCESS MESSAGE — Ipakita ang BUONG ADDRESS
            lblIDNumber.Text = "ID: " & generatedID
            MessageBox.Show("INFORMATION SUCCESSFULLY SAVED!" & vbCrLf & "GENERATED ID: " & generatedID & vbCrLf & vbCrLf & "FULL ADDRESS: " & fullAddress, "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ClearForm()

        Catch ex As Exception
            lblAddressError.Text = "ERROR: " & ex.Message
        Finally
            DBconnection.CloseConnection()
        End Try
    End Sub

    ' ==================================================
    ' ✅ BROWSE PHOTO BUTTON
    ' ==================================================
    Private Sub btnBrowsePhoto_Click(sender As Object, e As EventArgs) Handles btnBrowsePhoto.Click
        Using ofd As New OpenFileDialog()
            ofd.Title = "SELECT RESIDENT PHOTO"
            ofd.Filter = "IMAGE FILES|*.JPG;*.JPEG;*.PNG;*.BMP"
            If ofd.ShowDialog() = DialogResult.OK Then
                Using tempImg As Image = Image.FromFile(ofd.FileName)
                    picResident.Image = New Bitmap(tempImg)
                End Using
                picResident.SizeMode = PictureBoxSizeMode.Zoom
            End If
        End Using
    End Sub

    ' ==================================================
    ' ✅ CLEAR FORM — BURAHIN LAHAT, PERO BALIKAN ANG BARANGAY AT MUNICIPALITY
    ' ==================================================
    Private Sub ClearForm()
        txtLastname.Clear()
        txtFirstname.Clear()
        txtMI.Clear()
        cmbSuffix.SelectedIndex = -1
        cmbGender.SelectedIndex = -1
        cmbCivilStatus.SelectedIndex = -1
        txtNationality.Text = "FILIPINO"
        txtEmail.Text = "@gmail.com"
        txtPhone.Clear()
        txtAddress.Clear()
        txtAge.Clear()
        picResident.Image = Nothing
        dtpBirthDate.Value = Date.Today

        ' ✅ BALIKAN ULIT ANG BARANGAY AT MUNICIPALITY PAGKATAPUS BURAHIN
        txtBarangay.Text = "PUTATAN"
        txtMunicipality.Text = "MUNTINLUPA CITY"

        ClearValidation()
        lblIDNumber.Text = "ID: WILL BE GENERATED"
    End Sub

    ' ==================================================
    ' ✅ CANCEL BUTTON
    ' ==================================================
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

End Class