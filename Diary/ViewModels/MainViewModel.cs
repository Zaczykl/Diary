using Diary.Commands;
using Diary.Models;
using Diary.Views;
using MahApps;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace Diary.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {

            AddStudentCommand = new RelayCommand(AddStudent);
            EditStudentCommand = new RelayCommand(EditStudent,CanEditDeleteStudent);
            DeleteStudentCommand = new AsyncRelayCommand(DeleteStudent, CanEditDeleteStudent);
            RefreshStudentsCommand = new RelayCommand(RefreshStudents);
            InitGroups();
            RefreshDiary();

        }    
        public ICommand RefreshStudentsCommand { get; set; }
        public ICommand AddStudentCommand { get; set; }
        public ICommand EditStudentCommand { get; set; }
        public ICommand DeleteStudentCommand { get; set; }
        
        private Student _selectedStudent;

        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set 
            { 
                _selectedStudent = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Student> _students;

        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set { _students = value; }
        }

        private int _selectedGroupId;

        public int SelectedGroupId
        {
            get { return _selectedGroupId; }
            set 
            {
                _selectedGroupId = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Group> _groups;

        public ObservableCollection<Group> Groups
        {
            get { return _groups; }
            set 
            { 
                _groups = value;
                OnPropertyChanged();
            }
        }
        private async Task DeleteStudent(object obj)
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            var dialog= await metroWindow.ShowMessageAsync
                ("Usuwanie ucznia", 
                $"Czy na pewno chcesz usunąc ucznia {SelectedStudent.FirstName} {SelectedStudent.LastName}?", 
                MessageDialogStyle.AffirmativeAndNegative);

            if (dialog!=MessageDialogResult.Affirmative)
                return;

            //tutaj będzie kod odpowiedzialny za usuwanie ucznia z bazy
            RefreshDiary();
        }

        private void AddStudent(object obj)
        {
            var addEditStudentWindow = new AddEditStudentView(obj as Student);
            addEditStudentWindow.Closed += AddEditStudentWindow_Closed;
            addEditStudentWindow.ShowDialog();
        }
        private void EditStudent(object obj)
        {
            var addEditStudentWindow = new AddEditStudentView(obj as Student);
            addEditStudentWindow.Closed += AddEditStudentWindow_Closed;
            addEditStudentWindow.ShowDialog();
        }

        private void AddEditStudentWindow_Closed(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private void RefreshStudents(object obj)
        {
            RefreshDiary();
        }
        private bool CanEditDeleteStudent(object obj)
        {
            return SelectedStudent != null;
        }
        private void InitGroups()
        {
            Groups = new ObservableCollection<Group>
            {
                new Group{Id=0,Name="Wszystkie"},
                new Group{Id=1,Name="1A"},
                new Group{Id=2,Name="1B"},
                new Group{Id=3,Name="1C"}
            };
            SelectedGroupId = 0;
        }
        private void RefreshDiary()
        {
            Students = new ObservableCollection<Student>
            {
                new Student
                {
                    FirstName="Łukasz",
                    LastName="Zaczyk",
                    Group=new Group{Id=0,Name="1A"}
                },
                new Student
                {
                    FirstName="Jan",
                    LastName="Kowalski",
                    Group=new Group{Id=1,Name="1B"}
                },
                new Student
                {
                    FirstName="Piotr",
                    LastName="Nowak",
                    Group=new Group{Id=2,Name="1C"}
                },
            };
        }

    }
}
