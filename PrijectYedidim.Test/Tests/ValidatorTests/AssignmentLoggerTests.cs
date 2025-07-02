using System;
using System.Collections.Generic;
using System.IO;
using Repository.Entites;
using Service.Algorithm.Logging;
using Xunit;

namespace Tests.ValidatorTests
{
    public class AssignmentLoggerTests
    {
        private readonly List<(int messageId, int volunteerId)> _assignments;
        private readonly List<Volunteer> _volunteers;
        private readonly List<Message> _messages;

        public AssignmentLoggerTests()
        {
            _assignments = new List<(int, int)>
            {
                (1, 100),
                (2, 101)
            };

            _volunteers = new List<Volunteer>
            {
                new Volunteer { volunteer_id = 100, volunteer_first_name = "Alice" },
                new Volunteer { volunteer_id = 101, volunteer_first_name = "Bob" }
            };

            _messages = new List<Message>
            {
                new Message { message_id = 1, helped_id = 1 },
                new Message { message_id = 2, helped_id = 2 },
                new Message { message_id = 3, helped_id = 3 } // לא שובצה
            };
        }

        [Fact]
        public void LogMatches_ShouldPrintAllMatches()
        {
            var sw = new StringWriter();
            Console.SetOut(sw);

            AssignmentLogger.LogMatches(_assignments, _messages, _volunteers);

            var output = sw.ToString();

            Assert.Contains("Matched Message ID: 1 (Helped ID: 1) -> Volunteer ID: 100 (Alice)", output);
            Assert.Contains("Matched Message ID: 2 (Helped ID: 2) -> Volunteer ID: 101 (Bob)", output);
        }

        [Fact]
        public void LogUnassigned_ShouldPrintUnmatchedMessages()
        {
            var sw = new StringWriter();
            Console.SetOut(sw);

            AssignmentLogger.LogUnassigned(_messages, _assignments);

            var output = sw.ToString();
            Assert.Contains("Call 3: NOT ASSIGNED (Reason: No eligible volunteer found)", output);
        }

        [Fact]
        public void LogTimestamp_ShouldPrintTimeRange()
        {
            var sw = new StringWriter();
            Console.SetOut(sw);

            var start = DateTime.Now;
            System.Threading.Thread.Sleep(50);
            AssignmentLogger.LogTimestamp("Test Phase", start, start);

            var output = sw.ToString();
            Assert.Contains("Test Phase completed", output);
        }

        [Fact]
        public void LogSummary_ShouldPrintAssignmentStats()
        {
            var sw = new StringWriter();
            Console.SetOut(sw);

            AssignmentLogger.LogSummary(_volunteers, _assignments, _messages);

            var output = sw.ToString();
            Assert.Contains("Volunteers used: 2/2", output);
            Assert.Contains("Calls assigned:  2/3", output);
        }
    }
}
