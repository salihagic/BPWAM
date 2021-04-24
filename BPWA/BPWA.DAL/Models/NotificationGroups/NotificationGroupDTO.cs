﻿namespace BPWA.DAL.Models
{
    public class NotificationGroupDTO : BaseDTO
    {
        public int NotificationId { get; set; }
        public int GroupId { get; set; }

        public NotificationDTO Notification { get; set; }
        public GroupDTO Group { get; set; }
    }
}