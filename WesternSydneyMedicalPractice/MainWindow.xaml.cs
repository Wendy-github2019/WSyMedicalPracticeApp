using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents; 
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;

namespace WesternSydneyMedicalPractice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Field Variables
        //Get all the Practitioners from the database
        Practitioners allPractitioners = new Practitioners();

        Patients allPatients = new Patients();

        Appointments allAppointments;
        #endregion Field Variables

        public MainWindow()
        {
            InitializeComponent();

            //First tiem the Window loads go into the Practitioner's Tab
            tabcontrolMain.SelectedItem = tabItemPractitioner;            

            LoadPractitionersListView();

            LoadPatientsListView();

        }

        #region Practitioner Tab

        private void LoadPractitionersListView()
        {
            //bind the practitioner list 
            lvPractitioners.ItemsSource = allPractitioners;
        }

        #region Practitioners Event Handlers
        private void tabcontrolMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }

        private void lvPractitioners_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }

        private void btnFirstRecord_Click(object sender, RoutedEventArgs e)
        {
            //check if the ListView has Items in it first
            if (lvPractitioners.Items.Count > 0)
            {
                lvPractitioners.SelectedIndex = 0;
                lvPractitioners.ScrollIntoView(lvPractitioners.SelectedItem);
            }
        }

        private void btnPreviousRecord_Click(object sender, RoutedEventArgs e)
        {
            if ((lvPractitioners.Items.Count > 0) && (lvPractitioners.SelectedIndex > 0))
            {
                lvPractitioners.SelectedIndex -= 1;
                lvPractitioners.ScrollIntoView(lvPractitioners.SelectedItem);
            }
        }

        private void btnNextRecord_Click(object sender, RoutedEventArgs e)
        {
            if ( (lvPractitioners.Items.Count > 0) && (lvPractitioners.SelectedIndex < lvPractitioners.Items.Count - 1))
            {
                lvPractitioners.SelectedIndex += 1;

                lvPractitioners.ScrollIntoView(lvPractitioners.SelectedItem);
            }
        }

        private void btnLastRecord_Click(object sender, RoutedEventArgs e)
        {
            if (lvPractitioners.Items.Count > 0)
            {
                lvPractitioners.SelectedIndex = lvPractitioners.Items.Count - 1;
                lvPractitioners.ScrollIntoView(lvPractitioners.SelectedItem);
            }
        }

        private void cboPracType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //prevent recursive calls to this event
            e.Handled = true;
        }

        private void txtState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }

        #endregion Practitioners Event Handlers


        private void btnAddPractitioner_Click(object sender, RoutedEventArgs e)
        {
            //Toggle the button between "Add New" and "Save"
            if (btnAddPractitioner.Content.ToString() == "Add new")
            {
                //Change the UI to add a new Practitioner
                //Toggle the button, change to Save
                btnAddPractitioner.Content = "Save";
                //Deselect the ListView item
                lvPractitioners.SelectedIndex = -1;
                lvPractitioners.IsEnabled = false;
                //clear the controls
                clearPractitionerTabControls();

                //Show the cancel button and diaable the Update, Delete and Navigation button
                btnCancelPractitioner.IsEnabled = true;
                btnUpdatePractitioner.IsEnabled = false;
                btnDeletePractitioner.IsEnabled = false;
                btnPreviousRecord.IsEnabled = false;
                btnNextRecord.IsEnabled = false;
                btnLastRecord.IsEnabled = false;
                btnFirstRecord.IsEnabled = false;   
            }
            else //Do the save of the Practitioner
            {
                //Check if the controls Validate (that we have data in controls)
                if (ValidatePracControls())
                {
                    //populate a practitioner

                    Practitioner newPractitioner = new Practitioner();

                    AssignPropertiesToPraction(newPractitioner);                      

                    if (newPractitioner.Insert() == 1)
                    {
                        MessageBox.Show("New Practitioner's details have been successfully saved!", "Details Saved", MessageBoxButton.OK, MessageBoxImage.Information);

                        RefreshPractionersList();
                        lvPractitioners.SelectedIndex = lvPractitioners.Items.Count - 1;
                        lvPractitioners.ScrollIntoView(lvPractitioners.SelectedIndex);
                    }
                    else
                    {
                        MessageBox.Show("New Practitioner's details were not saved!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    btnAddPractitioner.Content = "Add new";
                    //diable the cancel button and show the Update, Delete and Navigation button
                    btnCancelPractitioner.IsEnabled = false;
                    btnUpdatePractitioner.IsEnabled = true;
                    btnDeletePractitioner.IsEnabled = true;
                    lvPractitioners.IsEnabled = true;
                    btnPreviousRecord.IsEnabled = true;
                    btnNextRecord.IsEnabled = true;
                    btnLastRecord.IsEnabled = true;
                    btnFirstRecord.IsEnabled = true;
                }
            }
        }

        private void RefreshPractionersList()
        {
            allPractitioners = null;
            allPractitioners = new Practitioners();
            LoadPractitionersListView();
        }

        private bool ValidatePracControls()
        {
            if (cboPracType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Practition Type!", "Practitioner Type?", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                cboPracType.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtPracFirstName.Text))
            {
                MessageBox.Show("Please enter first name", "first name?", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPracFirstName.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtPracLastName.Text))
            {
                MessageBox.Show("Please enter Last name", "Last name?", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPracLastName.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtPracStreet.Text))
            {
                MessageBox.Show("Please enter a Street", "Street?", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPracStreet.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtPracSuburb.Text))
            {
                MessageBox.Show("Please enter a Suburb", "Suburb?", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPracSuburb.Focus();
                return false;
            }
            else if (comPracState.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter a State", "State?", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                comPracState.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtPracPostCode.Text))
            {
                MessageBox.Show("Please enter a PostCode", "PostCode?", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPracPostCode.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtPracMobile.Text))
            {
                MessageBox.Show("Please enter a Mobile number", "Mobile?", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPracMobile.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtPracHomePhone.Text))
            {
                MessageBox.Show("Please enter a Home Phone", "Home Phone?", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPracHomePhone.Focus();
                return false;
            }  
            else if (string.IsNullOrEmpty(txtPracRegistrationNumber.Text))
            {
                MessageBox.Show("Please enter a Registration Number", "Registration Number?", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPracRegistrationNumber.Focus();
                return false;
            }
            else
            {
                return true;
            }
        }

        private void clearPractitionerTabControls()
        {
            txtPractitioner_ID.Clear();
            txtPracFirstName.Clear();
            txtPracLastName.Clear();
            txtPracStreet.Clear();
            txtPracSuburb.Clear();
            comPracState.SelectedIndex = -1;
            txtPracPostCode.Clear();
            txtPracHomePhone.Clear();
            txtPracMobile.Clear();
            txtPracRegistrationNumber.Clear();
            cboPracType.SelectedIndex = -1;
        }

        private void btnCancelPractitioner_Click(object sender, RoutedEventArgs e)
        {
            btnCancelPractitioner.IsEnabled = false;
            btnAddPractitioner.Content = "Add new";
            lvPractitioners.IsEnabled = true;

            btnUpdatePractitioner.IsEnabled = true;
            btnDeletePractitioner.IsEnabled = true;    
            //Navigation buttons
            btnPreviousRecord.IsEnabled = true;
            btnNextRecord.IsEnabled = true;
            btnLastRecord.IsEnabled = true;
            btnFirstRecord.IsEnabled = true;

            //Clear the controls
            clearPractitionerTabControls();
            LoadPractitionersListView();
            lvPractitioners.SelectedIndex = 0;
            lvPractitioners.ScrollIntoView(lvPractitioners.SelectedIndex);

        }

        private void btnDeletePractitioner_Click(object sender, RoutedEventArgs e)
        {
            //Ask if user really want to delete
            if (lvPractitioners.SelectedItem != null)
            {
                string message = "Are you really, really sure that you want to DELETE this practitioner?" + Environment.NewLine + "The Practitioner Details and all appointments will be permanently DELETED?";
                string caption = "Delete Practitioner?";

                if (MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    //if yes
                    //instantiate a Practitioner, and make the object the one that's selected in listview
                    Practitioner selectedPractitioner = (Practitioner)lvPractitioners.SelectedItem;
                    
                    try
                    {
                        //check if Delete method return 1, if it's 1, means successfully, show message      
                        if (selectedPractitioner.Delete() == 1)
                        {
                            MessageBox.Show("Practitioner successfully Deleted from the database! Now you're REALLY in trouble", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            RefreshPractionersList();
                            //Go to the first item in Practitioner list, because there is just a delete.
                            lvPractitioners.SelectedIndex = 1;
                            lvPractitioners.ScrollIntoView(lvPractitioners.SelectedItem);
                        }
                    }
                    catch (Exception ex)
                    {
                        message = "Something went wrong" + Environment.NewLine + "The Practitions's details were not deleted" + Environment.NewLine + ex.Message;
                        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }

            }
            else
            {
                MessageBox.Show("Please first select a Practitioner to be deleted.", "Select a Practitioner", MessageBoxButton.OK);
            }
      
        }

        private void btnUpdatePractitioner_Click(object sender, RoutedEventArgs e)
        {
            //fisrt check the one of the Practitioners is selected in the ListView
            if (lvPractitioners.SelectedItem != null)
            {
                string message = "The Practitioner's details will be updated?" + Environment.NewLine + "Do you wish to continue?";
                string caption = "Update Practitioner?";

                if (MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    //Update the Pratitioner

                    //Instantiate the Practitioner object that is selected in the ListView
                    //in the ListView
                    Practitioner selectedPractitioner = (Practitioner)lvPractitioners.SelectedItem;
                    int selectedIndex = lvPractitioners.SelectedIndex;

                    AssignPropertiesToPraction(selectedPractitioner);
                    //Wrap the update in a Try-catch
                    try
                    {
                        if (selectedPractitioner.Update() == 1)
                        {
                            MessageBox.Show("Practitioner's details successfully updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            RefreshPractionersList();
                            //Go back to the Practitioner that was just updated.
                            lvPractitioners.SelectedIndex = selectedIndex;
                            lvPractitioners.ScrollIntoView(lvPractitioners.SelectedItem);
                        }

                    }
                    catch (Exception ex)
                    {
                        message = "Something went wrong" + Environment.NewLine + "The Practitions's details were not updated" + Environment.NewLine + ex.Message;
                        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }       
            }
            else 
            {
                MessageBox.Show("Please first select a Practitioner to be updated.", "Select a Practitioner", MessageBoxButton.OK);
            }
        }

        private void AssignPropertiesToPraction(Practitioner practitioner)
        {
            practitioner.LoadPractitionerProperties(
                    txtPracFirstName.Text,
                    txtPracLastName.Text,
                    txtPracStreet.Text,
                    txtPracSuburb.Text,
                    comPracState.Text,
                    txtPracPostCode.Text,
                    txtPracHomePhone.Text,
                    txtPracMobile.Text,
                    txtPracRegistrationNumber.Text,
                    cboPracType.Text
                );
            practitioner.SetAvaibleDay(
                        chkBxMonday.IsChecked.Value,
                        (bool)chkBxTuesday.IsChecked,
                        chkBxWednesday.IsChecked.Value,
                        chkBxThursday.IsChecked.Value,
                        chkBxFriday.IsChecked.Value,
                        chkBxSaturday.IsChecked.Value,
                        chkBxSunday.IsChecked.Value
                );
        }

        #endregion Practitioner Tab

        #region Patient_Tab

        private void LoadPatientsListView()
        {
            lvPatients.ItemsSource = allPatients; 
        }

        private void RefreshPatientsList()
        {
            allPatients = null;
            allPatients = new Patients();
            LoadPatientsListView();
        }

        #region Patients Event Handlers
        private void lvPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }

        private void btnPatFirstRecord_Click(object sender, RoutedEventArgs e)
        {
            if (lvPatients.Items.Count > 0)
            {
                lvPatients.SelectedIndex = 0;
                lvPatients.ScrollIntoView(lvPatients.SelectedItem);
            }
        }

        private void btnPatPreviousRecord_Click(object sender, RoutedEventArgs e)
        {
            if ((lvPatients.Items.Count > 0) && (lvPatients.SelectedIndex > 0))
            {
                lvPatients.SelectedIndex -= 1;
                lvPatients.ScrollIntoView(lvPatients.SelectedItem);
            }
        }

        private void btnPatNextRecord_Click(object sender, RoutedEventArgs e)
        {
            if ((lvPatients.Items.Count > 0) && (lvPatients.SelectedIndex < lvPatients.Items.Count - 1))
            {
                lvPatients.SelectedIndex += 1;
                lvPatients.ScrollIntoView(lvPatients.SelectedItem);
            }
        }

        private void btnPatLastRecord_Click(object sender, RoutedEventArgs e)
        {
            if (lvPatients.Items.Count > 0)
            {
                lvPatients.SelectedIndex = lvPatients.Items.Count - 1;
                lvPatients.ScrollIntoView(lvPatients.SelectedItem);
            }
        }
        #endregion

        private void btnAddNewPatient_Click(object sender, RoutedEventArgs e)
        {
            //Toggle the button between "Add New" and "Save"
            if (btnAddNewPatient.Content.ToString() == "Add New")
            {
                btnAddNewPatient.Content = "Save";
                //Deselect the ListView item
                lvPatients.SelectedIndex = -1;
                lvPatients.IsEnabled = false;
                //clear the controls
                clearPatientTabControls();

                btnCancelPatient.IsEnabled = true;
                btnUpdatePatient.IsEnabled = false;
                btnDeletePatient.IsEnabled = false;
                btnPatFirstRecord.IsEnabled = false;
                btnPatLastRecord.IsEnabled = false;
                btnPatPreviousRecord.IsEnabled = false;
                btnPatNextRecord.IsEnabled = false;
            }
            else //Do the save of the Patient
            {
                //check if having data in controls
                if (ValidatePatiectControls())
                {
                    //populate a patient
                    Patient newPatient = new Patient();
                    AssignPropertiesToPatient(newPatient);

                    if (newPatient.Insert() == 1)
                    {
                        MessageBox.Show("New Patient's details have been successfully saved!", "Details Saved", MessageBoxButton.OK, MessageBoxImage.Information);

                        RefreshPatientsList();
                        lvPatients.SelectedIndex = lvPatients.Items.Count - 1;
                        lvPatients.ScrollIntoView(lvPatients.SelectedIndex);
                    }
                    else
                    {
                        MessageBox.Show("New Patient's details were not saved!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    btnAddNewPatient.Content = "Add New";
                    //diable the cancel button and show the Update, Delete and Navigation button

                    btnCancelPatient.IsEnabled = false;
                    lvPatients.IsEnabled = true;
                    btnUpdatePatient.IsEnabled = true;
                    btnDeletePatient.IsEnabled = true;
                    //Navigation buttons
                    btnPatFirstRecord.IsEnabled = true;
                    btnPatLastRecord.IsEnabled = true;
                    btnPatPreviousRecord.IsEnabled = true;
                    btnPatNextRecord.IsEnabled = true;
                }
            }
        }

        private void AssignPropertiesToPatient(Patient patient)
        {
            patient.Gender = cboGender.Text;
            patient.DateOfBirth = dtpPatDateOfBirth.SelectedDate.Value;
            patient.FirstName = txtPatFirstName.Text;
            patient.LastName = txtPatLastName.Text;
            patient.Street = txtPatStreet.Text;
            patient.Suburb = txtPatSuburb.Text;
            patient.State = cboPatState.Text;
            patient.PostCode = txtPatPostCode.Text;
            patient.HomePhone = txtPatHomePhone.Text;
            patient.Mobile = txtPatMobilePhone.Text;
            patient.MedicalNumber = txtPatMedicareNumber.Text;
            patient.PatientNote = txtPatNotes.Text;
        }

        private bool ValidatePatiectControls()
        {
            if (cboGender.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Patient gender!", "Patient gender?", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                cboGender.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtPatFirstName.Text))
            {
                MessageBox.Show("Please enter first name", "first name?", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPatFirstName.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtPatLastName.Text))
            {
                MessageBox.Show("Please enter Last name", "Last name?", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPatLastName.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtPatStreet.Text))
            {
                MessageBox.Show("Please enter a Street", "Street?", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPatStreet.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtPatSuburb.Text))
            {
                MessageBox.Show("Please enter a Suburb", "Suburb?", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPatSuburb.Focus();
                return false;
            }
            else if (cboPatState.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter a State", "State?", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                cboPatState.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtPatPostCode.Text))
            {
                MessageBox.Show("Please enter a PostCode", "PostCode?", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPatPostCode.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtPatHomePhone.Text))
            {
                MessageBox.Show("Please enter a Homephone number", "Phone?", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPracHomePhone.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtPatMobilePhone.Text))
            {
                MessageBox.Show("Please enter a Mobile Phone", "Mobile Phone?", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPracMobile.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtPatMedicareNumber.Text))
            {
                MessageBox.Show("Please enter a Patient Medicare Number", "Medicare Number?", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPatMedicareNumber.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtPatNotes.Text))
            {
                MessageBox.Show("Please enter a Patient Note", "Patient Note?", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPatMedicareNumber.Focus();
                return false;
            }
            else
            {
                return true;
            }
        }

        private void clearPatientTabControls()
        {
            txtPatient_ID.Clear();
            cboGender.SelectedIndex = -1;
            txtPatFirstName.Clear();
            txtPatLastName.Clear();
            txtPatHomePhone.Clear();
            txtPatStreet.Clear();
            txtPatSuburb.Clear();
            cboPatState.SelectedIndex = -1;
            txtPatPostCode.Clear();
            txtPatMobilePhone.Clear();
            txtPatMedicareNumber.Clear();
            dtpPatDateOfBirth.SelectedDate = null;
            txtPatNotes.Clear();

        }

        private void btnUpdatePatient_Click(object sender, RoutedEventArgs e)
        {
            //fisrt check the one of the Practitioners is selected in the ListView
            if (lvPatients.SelectedItem != null)
            {
                string message = "The Patient's details will be updated?" + Environment.NewLine + "Do you wish to continue?";
                string caption = "Update Patient?";

                if (MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    //Update the Patient

                    //Instantiate the Patient object that is selected in the ListView
                    //in the ListView
                    Patient selectedPatient = (Patient)lvPatients.SelectedItem;
                    int selectedIndex = lvPatients.SelectedIndex;

                    selectedPatient.Patient_ID = int.Parse(txtPatient_ID.Text);
                    AssignPropertiesToPatient(selectedPatient);
                    //Wrap the update in a Try-catch
                    try
                    {
                        if (selectedPatient.Update() == 1)
                        {
                            MessageBox.Show("Patient's details successfully updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            RefreshPatientsList();
                            //Go back to the Practitioner that was just updated.
                            lvPatients.SelectedIndex = selectedIndex;
                            lvPatients.ScrollIntoView(lvPatients.SelectedItem);
                        }

                    }
                    catch (Exception ex)
                    {
                        message = "Something went wrong" + Environment.NewLine + "The Patient's details were not updated" + Environment.NewLine + ex.Message;
                        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please first select a Practitioner to be updated.", "Select a Practitioner", MessageBoxButton.OK);
            }
        }

        private void btnDeletePatient_Click(object sender, RoutedEventArgs e)
        {
            //Ask if user really want to delete
            if (lvPatients.SelectedItem != null)
            {
                string message = "Are you really, really sure that you want to DELETE this patient?" + Environment.NewLine + "The Patient Details and all appointments will be permanently DELETED?";
                string caption = "Delete Patient?";

                if (MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    //if yes
                    //instantiate a Patient, and make the object the one that's selected in listview
                    Patient selectedPatient = (Patient)lvPatients.SelectedItem;

                    try
                    {
                        //check if Delete method return 1, if it's 1, means successfully, show message      
                        if (selectedPatient.Delete() == 1)
                        {
                            MessageBox.Show("Patient successfully Deleted from the database! Now you're REALLY in trouble", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            RefreshPatientsList();
                            //Go to the first item in Practitioner list, because there is just a delete.
                            lvPatients.SelectedIndex = 1;
                            lvPatients.ScrollIntoView(lvPatients.SelectedItem);
                        }
                    }
                    catch (Exception ex)
                    {
                        message = "Something went wrong" + Environment.NewLine + "The Patient's details were not deleted" + Environment.NewLine + ex.Message;
                        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void btnCancelPatient_Click(object sender, RoutedEventArgs e)
        {
            btnCancelPatient.IsEnabled = false;
            btnAddNewPatient.Content = "Add New";
            lvPatients.IsEnabled = true;
            lvPatients.SelectedIndex = 0;
            lvPatients.ScrollIntoView(lvPatients.SelectedItem);

            btnUpdatePatient.IsEnabled = true;
            btnDeletePatient.IsEnabled = true;
            //Navigation buttons
            btnPatFirstRecord.IsEnabled = true;
            btnPatLastRecord.IsEnabled = true;
            btnPatPreviousRecord.IsEnabled = true;
            btnPatNextRecord.IsEnabled = true;
        }

        private void btnAppointmentsPatient_Click(object sender, RoutedEventArgs e)
        {
            if (lvPatients.SelectedItem != null)
            {
                Patient patient = (Patient)lvPatients.SelectedItem;
                allAppointments = new Appointments(patient);

                lvAppointmt.ItemsSource = allAppointments; 
            }
        }

        private void cboGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cboPatState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        #endregion Patient_Tab

        private void lvAppointmt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
