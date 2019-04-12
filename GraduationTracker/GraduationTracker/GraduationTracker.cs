using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationTracker
{
    public static class GraduationTracker
    {   
        public static Tuple<bool, STANDING> HasGraduated(Diploma diploma, Student student)
        {
            int average = CalculateAverage(diploma, student) / student.Courses.Length;
            bool reqCreditsEarned = RequiredCreditsEarned(diploma, student);
            student.Standing = DetermineStanding(average, reqCreditsEarned);

            return new Tuple<bool, STANDING>(reqCreditsEarned && student.Standing > STANDING.Remedial, student.Standing);
        }

        private static bool RequiredCreditsEarned(Diploma diploma, Student student)
        {
            return student.Credits >= diploma.Credits;
        }

        private static STANDING DetermineStanding(int average, bool reqCreditsEarned)
        {
            STANDING standing;
            if (reqCreditsEarned) {
                if (average < 50)
                    standing = STANDING.Remedial;
                else if (average < 80)
                    standing = STANDING.Average;
                else if (average < 95)
                    standing = STANDING.MagnaCumLaude;
                else
                    standing = STANDING.SumaCumLaude;
            } else
                standing = STANDING.Remedial;
            return standing;
        }

        private static int CalculateAverage(Diploma diploma, Student student)
        {
            int average = 0;

            for (int i = 0; i < diploma.Requirements.Length; i++)
            {
                for (int j = 0; j < student.Courses.Length; j++)
                {
                    var requirement = Repository.GetRequirement(diploma.Requirements[i]);

                    for (int k = 0; k < requirement.Courses.Length; k++)
                    {
                        if (requirement.Courses[k] == student.Courses[j].Id)
                        {
                            average += student.Courses[j].Mark;

                            if (student.Courses[j].Mark >= requirement.MinimumMark)
                            {
                                student.Credits += requirement.Credits;
                            }
                        }
                    }
                }
            }

            return average;
        }
    }
}
