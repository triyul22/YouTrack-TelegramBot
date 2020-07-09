using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using YouTrackSharp;
using YouTrackSharp.Issues;

namespace TelegramTaskTrackerBot
{
    class YouTrackAccount
    {

        private BearerTokenConnection _connection;

        public List<Change> changesList { get; set; }

        public ICollection<YouTrackSharp.Projects.Project> Projects { get; private set; }

        public ICollection<YouTrackSharp.Issues.Issue> Issues { get; private set; }

        public ICollection<YouTrackSharp.Issues.Comment> Comments { get; private set; }

        private YouTrackAccount(string url, string token)
        {
            _connection = new BearerTokenConnection(url, token);
        }

        /// <summary>
        /// Create an object of class
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<YouTrackAccount> create(string url, string token)
        {
            var account = new YouTrackAccount(url, token);

            account.changesList = new List<Change>();
            account.Projects = await account._connection.CreateProjectsService().GetAccessibleProjects();

            account.Issues = await account._connection.CreateIssuesService().GetIssues();
            account.Comments = new List<YouTrackSharp.Issues.Comment>();

            foreach(var issue in account.Issues)
            {
                foreach(var comment in issue.Comments)
                { 
                    account.Comments.Add(comment);
                }
            }
            return account;
        }

        /// <summary>
        /// Check new issues appear
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<YouTrackSharp.Issues.Issue>> GetIssuesUpdates()
        {

            var newIssueList = await _connection.CreateIssuesService().GetIssues();

            ICollection<YouTrackSharp.Issues.Issue> list = newIssueList.Except(Issues, new ObjectComparer()).ToList();

            foreach (var item in list)
            {
                Issues.Add(item);
                changesList.Add(new Change($"You have new Issue {item.Id}"));
            }

            return list;
        }

        /// <summary>
        /// Getting new comments
        /// </summary>
        public async void GetCommentsUpdates()
        {
            var newIssue = await _connection.CreateIssuesService().GetIssues();
            foreach(var issue in newIssue)
            {
                foreach(var comment in issue.Comments)
                {
                    if (Comments.Any(c => c.Id == comment.Id)) continue;
                    changesList.Add(new Change($"You have a new comment from {comment.Author} on issue {comment.IssueId} with text \"{comment.Text}\""));
                    Comments.Add(comment);
                    //Issues.Add(await _connection.CreateIssuesService().GetIssue(comment.IssueId));
                }
            }
        }

        /// <summary>
        /// Getting updates for issues states
        /// </summary>
        /// <returns></returns>
        public async Task GetIssueStatusUpdate()
        {
            
            foreach (var issue in Issues)
            {
                var newIsssue = await _connection.CreateIssuesService().GetIssue(issue.Id);
                var newIssueState = (List<string>)newIsssue.GetField("State").Value;
                var oldIssueState = (List<string>)issue.GetField("State").Value;

                if (newIssueState[0] != oldIssueState[0])
                {
                    changesList.Add(new Change($"{issue.Id} has status changed from {oldIssueState[0]} to {newIssueState[0]}"));
                    Console.WriteLine($"{issue.Id} has status changed from {oldIssueState[0]} to {newIssueState[0]}");
                    issue.SetField("State", newIssueState);
                }
            }
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<YouTrackSharp.Management.User>> GetUsers()
        {
            return await _connection.CreateUserManagementService().GetUsers();
        }

    }

    /// <summary>
    /// Helper class to compare issues by Id
    /// </summary>
    class ObjectComparer : IEqualityComparer<YouTrackSharp.Issues.Issue>
    {

        public bool Equals([AllowNull] YouTrackSharp.Issues.Issue x, [AllowNull] YouTrackSharp.Issues.Issue y)
        {
            if (x.Id == y.Id) return true;
            return false;
        }


        public int GetHashCode([DisallowNull] YouTrackSharp.Issues.Issue obj)
        {
            return 0;
        }
    }
}
