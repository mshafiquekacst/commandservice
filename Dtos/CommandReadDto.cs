﻿namespace CommandService.Dtos
{
    public class CommandReadDto
    {
        public int Id { get; set; }
        public int HowTo { get; set; }
        public string CommandLine { get; set; }
        public int PlatformId { get; set; }
    }
}
