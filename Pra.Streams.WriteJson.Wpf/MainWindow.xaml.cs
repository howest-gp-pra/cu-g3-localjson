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
using Pra.Streams.WriteJson.Core;

namespace Pra.Streams.WriteJson.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        AppService appService;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            appService = new AppService();
            lstStudents.ItemsSource = appService.Students;
            lstAvailableCourses.ItemsSource = appService.Courses;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            appService.WriteJsons();
        }

        private void LstStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tblStudentInfo.Text = "";
            lstEnrollments.ItemsSource = null;
            lstEnrollments.Items.Refresh();

            if(lstStudents.SelectedItem != null)
            {
                Student student = (Student)lstStudents.SelectedItem;
                tblStudentInfo.Text = student.DisplayInfo();
                DisplayEnrollments(student);            
            }
        }
        private void DisplayEnrollments(Student student)
        {
            List<Enrollment> studentEnrollment = new List<Enrollment>();
            foreach (Enrollment enrollment in appService.Enrollments)
            {
                if (enrollment.Student == student)
                {
                    studentEnrollment.Add(enrollment);
                }
            }
            lstEnrollments.ItemsSource = studentEnrollment;
            lstEnrollments.Items.Refresh();
        }

        private void BtnEnroll_Click(object sender, RoutedEventArgs e)
        {
            if (lstStudents.SelectedItem == null) return;
            if (lstAvailableCourses.SelectedItem == null) return;
            Student student = (Student)lstStudents.SelectedItem;
            Course course = (Course)lstAvailableCourses.SelectedItem;
            Enrollment enrollment = new Enrollment(student, course);
            appService.AddEnrollment(enrollment);
            DisplayEnrollments(student);

        }

        private void btnUnEnroll_Click(object sender, RoutedEventArgs e)
        {
            if (lstEnrollments.SelectedItem == null) return;
            Student student = (Student)lstStudents.SelectedItem;
            Enrollment enrollment = (Enrollment)lstEnrollments.SelectedItem;
            appService.RemoveEnrollment(enrollment);
            DisplayEnrollments(student);
        }
    }
}
