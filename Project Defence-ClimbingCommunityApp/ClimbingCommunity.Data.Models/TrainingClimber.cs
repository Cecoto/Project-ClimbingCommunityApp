﻿namespace ClimbingCommunity.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Entity for the many to many relation between climbers and trainings.
    /// </summary>
    public class TrainingClimber
    {
        public string ClimberId { get; set; } = null!;
        public virtual Climber Climber { get; set; } = null!;

        public Guid TrainingId { get; set; }
        public virtual Training Training { get; set; } = null!;
    }
}
