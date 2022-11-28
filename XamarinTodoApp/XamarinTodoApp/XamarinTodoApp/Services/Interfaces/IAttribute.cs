using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinTodoApp.Services.Interfaces
{
    public interface IAttribute
    {
        int Level { get; set; }
        int CurrentExperience { get; set; }
        int ExperienceToNextLevel { get; set; }
    }
}
