﻿namespace MonacoTest.Shared {
    public class JudgeRequest {
        public string? TaskId { get; set; }
        public string? Code { get; set; }
        public string? Language { get; set; }
        public string[]? TestInputs { get; set; }
        public string[]? TestOutputs { get; set; }
    }
}