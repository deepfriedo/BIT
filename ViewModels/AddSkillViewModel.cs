using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Models;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.ViewModels
{
    class AddSkillViewModel : ContractorViewModel
    {
        private ObservableCollection<PreferredSkill> _preferredSkills;
        public ObservableCollection<PreferredSkill> PreferredSkills
        {
            get { return _preferredSkills; }
            set { _preferredSkills = value; OnPropertyChanged("PreferredSkills"); }
        }

        private PreferredSkill _selectedPreferredSkill;
        public PreferredSkill SelectedPreferredSkill
        {
            get { return _selectedPreferredSkill; }
            set { _selectedPreferredSkill = value; OnPropertyChanged("SelectedPreferredSkill"); }
        }

        private MyCommand _addCommand;
        public MyCommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new MyCommand(this.AddMethod, true);
                }
                return _addCommand;
            }
            set
            {
                _addCommand = value;
            }
        }

        private MyCommand _removeCommand;
        public MyCommand RemoveCommand
        {
            get
            {
                if (_removeCommand == null)
                {
                    _removeCommand = new MyCommand(this.RemoveMethod, true);
                }
                return _removeCommand;
            }
            set
            {
                _removeCommand = value;
            }
        }

        private MyCommand _findAllCommand;
        public MyCommand FindAllCommand
        {
            get
            {
                if (_findAllCommand == null)
                {
                    _findAllCommand = new MyCommand(this.FindAllMethod, true);
                }
                return _findAllCommand;
            }
            set
            {
                _findAllCommand = value;
            }
        }

        public void AddMethod()
        {
            string sqlStr = "INSERT INTO PREFERRED_SKILL(ContractorId, SkillName) VALUES(" + SelectedContractor.ContractorId + ", '" + SelectedSkill.SkillName + "')";

            SQLHelper objHelper = new SQLHelper("BIT");
            objHelper.ExecuteNonQuery(sqlStr);
        }

        public void RemoveMethod()
        {
            string sqlStr = "DELETE FROM PREFERRED_SKILL WHERE ContractorId = " + SelectedContractor.ContractorId + " AND SkillName = '" + SelectedPreferredSkill.SkillName + "'";

            SQLHelper objHelper = new SQLHelper("BIT");
            objHelper.ExecuteNonQuery(sqlStr);
        }

        public void FindAllMethod()
        {
            Contractors allContractors = new Contractors();
            Contractors = new ObservableCollection<Contractor>(allContractors);
        }

        private ObservableCollection<Skill> _skills;
        public ObservableCollection<Skill> Skills
        {
            get { return _skills; }
            set { _skills = value; OnPropertyChanged("Skills"); }
        }

        public Skill SelectedSkill { get; set; }

        public AddSkillViewModel()
        {
            Skills allSkills = new Skills();
            Skills = new ObservableCollection<Skill>(allSkills);
        }
    }
}
