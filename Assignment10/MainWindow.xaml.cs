using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Data;
using System.Dynamic;

namespace Assignment10
{
    // Set up everything needed for this project
    public partial class MainWindow : Window
    {
        private Random random = new Random();
        private const int horizontalOffset = 150, hairOffset = 0, eyesOffset = 200, noseOffset = 400, mouthOffset = 600;
        private const string faceDirectory = "images/Guy", chmPath = "FaceHelp.chm", webPath = "https://jermnet.github.io/CSharpAssignment5/";
        private ImageManager imageManager = new ImageManager();

        private List<BitmapImage> hair = new List<BitmapImage>();
        private List<BitmapImage> eyes = new List<BitmapImage>();
        private List<BitmapImage> nose = new List<BitmapImage>();
        private List<BitmapImage> mouth = new List<BitmapImage>();
        private List<CommandHandler> hairCMDList = new List<CommandHandler>();
        private List<CommandHandler> eyesCMDList = new List<CommandHandler>();
        private List<CommandHandler> noseCMDList = new List<CommandHandler>();
        private List<CommandHandler> mouthCMDList = new List<CommandHandler>();
        private List<CommandHandler> helpCMDList = new List<CommandHandler>();
        private DBManager dbManager = new DBManager();
        DataTable dt;
        int current_primary_key = 0;
        string fn = "", ln = "", address = "";
        List<string> elements;
        int hobbyIndex = -1, jobIndex = -1;

        public MainWindow()
        {
            SetupImages();
            InitializeComponent();
            FillDataGrid();
            SetupHandlers();
            firstCanvas.Children.Clear();
            hobby.ItemsSource = dbManager.GetHobbies();
            job.ItemsSource = dbManager.GetJobs();
        }

        // Setup the handlers, I found that setting them up in a loop just does not work for whatever reason
        private void SetupHandlers()
        {
           hairCMDList.Add(new CommandHandler(() => imageManager.GetImage(firstCanvas, horizontalOffset, hairOffset, hair[0]), true));
            hairCMDList.Add(new CommandHandler(() => imageManager.GetImage(firstCanvas, horizontalOffset, hairOffset, hair[1]), true));
            hairCMDList.Add(new CommandHandler(() => imageManager.GetImage(firstCanvas, horizontalOffset, hairOffset, hair[2]), true));
            hairCMDList.Add(new CommandHandler(() => imageManager.GetImage(firstCanvas, horizontalOffset, hairOffset, hair[3]), true));
            
            eyesCMDList.Add(new CommandHandler(() => imageManager.GetImage(firstCanvas, horizontalOffset, eyesOffset, eyes[0]), true));
            eyesCMDList.Add(new CommandHandler(() => imageManager.GetImage(firstCanvas, horizontalOffset, eyesOffset, eyes[1]), true));
            eyesCMDList.Add(new CommandHandler(() => imageManager.GetImage(firstCanvas, horizontalOffset, eyesOffset, eyes[2]), true));
            eyesCMDList.Add(new CommandHandler(() => imageManager.GetImage(firstCanvas, horizontalOffset, eyesOffset, eyes[3]), true));
            
            noseCMDList.Add(new CommandHandler(() => imageManager.GetImage(firstCanvas, horizontalOffset, noseOffset, nose[0]), true));
            noseCMDList.Add(new CommandHandler(() => imageManager.GetImage(firstCanvas, horizontalOffset, noseOffset, nose[1]), true));
            noseCMDList.Add(new CommandHandler(() => imageManager.GetImage(firstCanvas, horizontalOffset, noseOffset, nose[2]), true));
            noseCMDList.Add(new CommandHandler(() => imageManager.GetImage(firstCanvas, horizontalOffset, noseOffset, nose[3]), true));
            
            mouthCMDList.Add(new CommandHandler(() => imageManager.GetImage(firstCanvas, horizontalOffset, mouthOffset, mouth[0]), true));
            mouthCMDList.Add(new CommandHandler(() => imageManager.GetImage(firstCanvas, horizontalOffset, mouthOffset, mouth[1]), true));
            mouthCMDList.Add(new CommandHandler(() => imageManager.GetImage(firstCanvas, horizontalOffset, mouthOffset, mouth[2]), true));
            mouthCMDList.Add(new CommandHandler(() => imageManager.GetImage(firstCanvas, horizontalOffset, mouthOffset, mouth[3]), true));

            helpCMDList.Add(new CommandHandler(() => Get_Doc_Help(), true));
            helpCMDList.Add(new CommandHandler(() => Get_Web_Help(), true));

            DataContext = new
            {
                hairCMD1 = hairCMDList[0],
                hairCMD2 = hairCMDList[1],
                hairCMD3 = hairCMDList[2],
                hairCMD4 = hairCMDList[3],
                eyesCMD1 = eyesCMDList[0],
                eyesCMD2 = eyesCMDList[1],
                eyesCMD3 = eyesCMDList[2],
                eyesCMD4 = eyesCMDList[3],
                noseCMD1 = noseCMDList[0],
                noseCMD2 = noseCMDList[1],
                noseCMD3 = noseCMDList[2],
                noseCMD4 = noseCMDList[3],
                mouthCMD1 = mouthCMDList[0],
                mouthCMD2 = mouthCMDList[1],
                mouthCMD3 = mouthCMDList[2],
                mouthCMD4 = mouthCMDList[3],
                helpCMD1 = helpCMDList[0],
                helpCMD2 = helpCMDList[1],
            };

            InputBindings.Add(new KeyBinding(hairCMDList[0], new KeyGesture(Key.D1, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(hairCMDList[1], new KeyGesture(Key.D2, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(hairCMDList[2], new KeyGesture(Key.D3, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(hairCMDList[3], new KeyGesture(Key.D4, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(eyesCMDList[0], new KeyGesture(Key.Q, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(eyesCMDList[1], new KeyGesture(Key.W, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(eyesCMDList[2], new KeyGesture(Key.E, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(eyesCMDList[3], new KeyGesture(Key.R, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(noseCMDList[0], new KeyGesture(Key.A, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(noseCMDList[1], new KeyGesture(Key.S, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(noseCMDList[2], new KeyGesture(Key.D, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(noseCMDList[3], new KeyGesture(Key.F, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(mouthCMDList[0], new KeyGesture(Key.Z, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(mouthCMDList[1], new KeyGesture(Key.X, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(mouthCMDList[2], new KeyGesture(Key.C, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(mouthCMDList[3], new KeyGesture(Key.V, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(helpCMDList[0], new KeyGesture(Key.H, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(helpCMDList[1], new KeyGesture(Key.J, ModifierKeys.Control)));
        }

        // Setup the images using a loop
        private void SetupImages()
        {
            for (int i = 1; i < 5; i++)
            {
                hair.Add(new BitmapImage(new Uri(faceDirectory + i + "P1.png", UriKind.Relative)));
                eyes.Add(new BitmapImage(new Uri(faceDirectory + i + "P2.png", UriKind.Relative)));
                nose.Add(new BitmapImage(new Uri(faceDirectory + i + "P3.png", UriKind.Relative)));
                mouth.Add(new BitmapImage(new Uri(faceDirectory + i + "P4.png", UriKind.Relative)));
            }
        }

        // Stuff for the face maker controls
        private void Combo_Changed(object sender, RoutedEventArgs e)
        {
            if (Combo.SelectedItem is ComboBoxItem selectedItem)
            {
                int value = int.Parse(selectedItem.Tag.ToString());
                imageManager.GetImage(firstCanvas, horizontalOffset, hairOffset, hair[value]);
            }
        }
        private void Eyes_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                int value = int.Parse(checkBox.Tag.ToString());
                imageManager.GetImage(firstCanvas, horizontalOffset, eyesOffset, eyes[value]);
            }
        }

        private void Nose_Click(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                int value = int.Parse(radioButton.Tag.ToString());
                imageManager.GetImage(firstCanvas, horizontalOffset, noseOffset, nose[value]);
            }
        }

        private void Value_Changed(object sender, RoutedEventArgs e)
        {
            imageManager.GetImage(firstCanvas, horizontalOffset, mouthOffset, mouth[(int)Slider.Value - 1]);
        }

        // Randomly get images
        private void Random_Click(object sender, RoutedEventArgs e)
        {
            imageManager.GetImage(firstCanvas, horizontalOffset, hairOffset, hair[random.Next(0, 4)]);
            imageManager.GetImage(firstCanvas, horizontalOffset, eyesOffset, eyes[random.Next(0, 4)]);
            imageManager.GetImage(firstCanvas, horizontalOffset, noseOffset, nose[random.Next(0, 4)]);
            imageManager.GetImage(firstCanvas, horizontalOffset, mouthOffset, mouth[random.Next(0, 4)]);
        }

        // Help stuff
        public void Help_Click(object sender, RoutedEventArgs e)
        {
            Get_Doc_Help();
        }

        public void Help_Click_2(object sender, RoutedEventArgs e)
        {
            Get_Web_Help();
        }

        public void Get_Doc_Help()
        {
            Process.Start(new ProcessStartInfo(chmPath) { UseShellExecute = true });
        }

        private void Get_Web_Help()
        {
            Process.Start(new ProcessStartInfo(webPath) { UseShellExecute = true });
        }

        // Get all the people using the dbmanager
        private void FillDataGrid()
        {

            dt = dbManager.GetAllPeople();
            gridPerson.ItemsSource = dt.DefaultView;
        }

        // Clear search text and fill the data grid so it doesn't have the stored search
        private void b_reset_Click(object sender, RoutedEventArgs e)
        {
            tb_search.Text = "";
            FillDataGrid();
        }

        // Get needed info from the XAML
        public void get_Info()
        {
            fn = tb_first.Text;
            ln = tb_last.Text;
            address = tb_address.Text;
            elements = new List<string>();
            for (int i = 0; i < firstCanvas.Children.Count; i++)
            {
                if (firstCanvas.Children[i] is Image image)
                {
                    elements.Add(image.Source.ToString());
                }
            }

            hobbyIndex = hobby.SelectedIndex;
            jobIndex = job.SelectedIndex;
        }

        // Add using the dbmanager, get info with that method 
        private void b_Add_Click(object sender, RoutedEventArgs e)
        {
            get_Info();

            // Add this record if values not empty
            if (fn != "" && ln != "" && address != "" && elements[0] != "" && elements[1] != "" && elements[2] != "" && elements[3] != "" && hobbyIndex != -1 && jobIndex != -1)
            {
                dbManager.AddPerson(fn, ln, address, elements[0], elements[1], elements[2], elements[3], hobbyIndex+1, jobIndex+1);
                FillDataGrid();
            }
            else
            {
                MessageBox.Show("A field is empty...enter all fields");
            }
            elements.Clear();
        }


        // Get the last name and then hand it to the db manager
        private void b_search_Click(object sender, RoutedEventArgs e)
        {
            string lastName = tb_search.Text.Trim();
            if (!string.IsNullOrEmpty(lastName))
            {
                gridPerson.ItemsSource = dbManager.SearchPerson(lastName).DefaultView;
            }
            else
            {
                MessageBox.Show("Enter a last name to search.");
            }
        }

        // Gets called when grid is selected
        private void gridPerson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // When we get here after deleting a row, we can't get the current row
            try
            {
                // Fetch the columns from the selected row
                current_primary_key = (int)(gridPerson.SelectedItem as DataRowView).Row.ItemArray[0];
                tb_first.Text = (string)(gridPerson.SelectedItem as DataRowView).Row.ItemArray[1];
                tb_last.Text = (string)(gridPerson.SelectedItem as DataRowView).Row.ItemArray[2];
                tb_address.Text = (string)(gridPerson.SelectedItem as DataRowView).Row.ItemArray[3];
            }
            catch
            {
                current_primary_key = -1;
                tb_first.Text = ":(";
                tb_last.Text = ":(";
                tb_address.Text = ":(";
            }
        }

        // Delete click, nothing had to be changed for this one
        private void b_delete_Click(object sender, RoutedEventArgs e)
        {
            if (gridPerson.SelectedItem != null)
            {
                int key = (int)(gridPerson.SelectedItem as DataRowView).Row.ItemArray[0];
                dbManager.DeletePerson(key);
                FillDataGrid();
            }
        }

        // Get info with that method, then hand to dbmanager
        private void b_update_Click(object sender, RoutedEventArgs e)
        {
            if (current_primary_key > -1)
            {
                get_Info();
                dbManager.UpdatePerson(current_primary_key, fn, ln, address, elements[0], elements[1], elements[2], elements[3], hobbyIndex+1, jobIndex+1);
                FillDataGrid();
                elements.Clear();
            }
        }
    }
}
