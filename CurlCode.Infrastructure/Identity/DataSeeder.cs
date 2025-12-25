using CurlCode.Domain.Entities.Community;
using CurlCode.Domain.Entities.Identity;
using CurlCode.Domain.Entities.Problems;
using CurlCode.Domain.Entities.Submissions;
using CurlCode.Domain.Enums;
using CurlCode.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CurlCode.Infrastructure.Identity;

public class DataSeeder
{
    public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Create test users (non-admin) for seeding
        var testUsers = await SeedTestUsersAsync(userManager);

        // Seed Topics
        await SeedTopicsAsync(context);

        // Seed Problems
        var problems = await SeedProblemsAsync(context);

        // Seed TestCases
        await SeedTestCasesAsync(context, problems);

        // Seed ProblemTopics
        await SeedProblemTopicsAsync(context, problems);

        // Seed Submissions
        await SeedSubmissionsAsync(context, problems, testUsers);

        // Seed Solutions
        var solutions = await SeedSolutionsAsync(context, problems, testUsers);

        // Seed SolutionLikes
        await SeedSolutionLikesAsync(context, solutions, testUsers);

        // Seed Comments
        var comments = await SeedCommentsAsync(context, solutions, testUsers);

        // Seed CommentLikes
        await SeedCommentLikesAsync(context, comments, testUsers);

        // Seed ProblemLikes
        await SeedProblemLikesAsync(context, problems, testUsers);

        await context.SaveChangesAsync();
    }

    private static async Task<List<ApplicationUser>> SeedTestUsersAsync(UserManager<ApplicationUser> userManager)
    {
        var users = new List<ApplicationUser>();
        var testUserData = new[]
        {
            new { UserName = "john_doe", Email = "johndoe@gmail.com", Password = "JohnDoe2024!" },
            new { UserName = "sarah_smith", Email = "sarahsmith@gmail.com", Password = "SarahSmith2024!" },
            new { UserName = "mike_johnson", Email = "mikejohnson@gmail.com", Password = "MikeJohnson2024!" },
            new { UserName = "emily_williams", Email = "emilywilliams@gmail.com", Password = "EmilyWilliams2024!" }
        };

        foreach (var userData in testUserData)
        {
            var existingUser = await userManager.FindByNameAsync(userData.UserName);
            if (existingUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = userData.UserName,
                    Email = userData.Email,
                    IsAdmin = false
                };
                await userManager.CreateAsync(user, userData.Password);
                users.Add(user);
            }
            else
            {
                users.Add(existingUser);
            }
        }

        return users;
    }

    private static async Task SeedTopicsAsync(ApplicationDbContext context)
    {
        if (await context.Topics.AnyAsync())
            return;

        var topics = new[]
        {
            new Topic { Name = "Arrays", Description = "Problems related to arrays and array manipulation" },
            new Topic { Name = "Strings", Description = "String manipulation and pattern matching problems" },
            new Topic { Name = "Dynamic Programming", Description = "DP problems and optimization techniques" },
            new Topic { Name = "Graphs", Description = "Graph algorithms and traversal" },
            new Topic { Name = "Trees", Description = "Tree data structures and algorithms" },
            new Topic { Name = "Sorting", Description = "Sorting algorithms and techniques" },
            new Topic { Name = "Searching", Description = "Search algorithms and binary search" },
            new Topic { Name = "Greedy", Description = "Greedy algorithm problems" },
            new Topic { Name = "Backtracking", Description = "Backtracking and recursion problems" },
            new Topic { Name = "Math", Description = "Mathematical problems and number theory" }
        };

        await context.Topics.AddRangeAsync(topics);
    }

    private static async Task<List<Problem>> SeedProblemsAsync(ApplicationDbContext context)
    {
        if (await context.Problems.AnyAsync())
            return await context.Problems.ToListAsync();

        var problems = new[]
        {
            new Problem
            {
                Title = "Two Sum",
                Description = "Given an array of integers nums and an integer target, return indices of the two numbers such that they add up to target.",
                Constraints = "You may assume that each input would have exactly one solution, and you may not use the same element twice.",
                Examples = "Input: nums = [2,7,11,15], target = 9\nOutput: [0,1]\nExplanation: Because nums[0] + nums[1] == 9, we return [0, 1].",
                Difficulty = DifficultyLevel.Easy,
                TimeLimitMs = 2000,
                MemoryLimitMb = 256
            },
            new Problem
            {
                Title = "Reverse Linked List",
                Description = "Given the head of a singly linked list, reverse the list, and return the reversed list.",
                Constraints = "The number of nodes in the list is the range [0, 5000].",
                Examples = "Input: head = [1,2,3,4,5]\nOutput: [5,4,3,2,1]",
                Difficulty = DifficultyLevel.Easy,
                TimeLimitMs = 2000,
                MemoryLimitMb = 256
            },
            new Problem
            {
                Title = "Longest Substring Without Repeating Characters",
                Description = "Given a string s, find the length of the longest substring without repeating characters.",
                Constraints = "0 <= s.length <= 5 * 10^4",
                Examples = "Input: s = \"abcabcbb\"\nOutput: 3\nExplanation: The answer is \"abc\", with the length of 3.",
                Difficulty = DifficultyLevel.Medium,
                TimeLimitMs = 3000,
                MemoryLimitMb = 512
            },
            new Problem
            {
                Title = "Median of Two Sorted Arrays",
                Description = "Given two sorted arrays nums1 and nums2 of size m and n respectively, return the median of the two sorted arrays.",
                Constraints = "nums1.length == m\nnums2.length == n\n0 <= m <= 1000\n0 <= n <= 1000",
                Examples = "Input: nums1 = [1,3], nums2 = [2]\nOutput: 2.00000\nExplanation: merged array = [1,2,3] and median is 2.",
                Difficulty = DifficultyLevel.Hard,
                TimeLimitMs = 5000,
                MemoryLimitMb = 512
            },
            new Problem
            {
                Title = "Valid Parentheses",
                Description = "Given a string s containing just the characters '(', ')', '{', '}', '[' and ']', determine if the input string is valid.",
                Constraints = "1 <= s.length <= 10^4",
                Examples = "Input: s = \"()\"\nOutput: true\nInput: s = \"()[]{}\"\nOutput: true",
                Difficulty = DifficultyLevel.Easy,
                TimeLimitMs = 2000,
                MemoryLimitMb = 256
            },
            new Problem
            {
                Title = "Best Time to Buy and Sell Stock",
                Description = "You are given an array prices where prices[i] is the price of a given stock on the ith day. You want to maximize your profit by choosing a single day to buy one stock and choosing a different day in the future to sell that stock.",
                Constraints = "1 <= prices.length <= 10^5\n0 <= prices[i] <= 10^4",
                Examples = "Input: prices = [7,1,5,3,6,4]\nOutput: 5\nExplanation: Buy on day 2 (price = 1) and sell on day 5 (price = 6), profit = 6-1 = 5.",
                Difficulty = DifficultyLevel.Easy,
                TimeLimitMs = 2000,
                MemoryLimitMb = 256
            },
            new Problem
            {
                Title = "Container With Most Water",
                Description = "You are given an integer array height of length n. There are n vertical lines drawn such that the two endpoints of the ith line are (i, 0) and (i, height[i]). Find two lines that together with the x-axis form a container, such that the container contains the most water.",
                Constraints = "n == height.length\n2 <= n <= 10^5\n0 <= height[i] <= 10^4",
                Examples = "Input: height = [1,8,6,2,5,4,8,3,7]\nOutput: 49",
                Difficulty = DifficultyLevel.Medium,
                TimeLimitMs = 3000,
                MemoryLimitMb = 512
            },
            new Problem
            {
                Title = "3Sum",
                Description = "Given an integer array nums, return all the triplets [nums[i], nums[j], nums[k]] such that i != j, i != k, and j != k, and nums[i] + nums[j] + nums[k] == 0.",
                Constraints = "3 <= nums.length <= 3000\n-10^5 <= nums[i] <= 10^5",
                Examples = "Input: nums = [-1,0,1,2,-1,-4]\nOutput: [[-1,-1,2],[-1,0,1]]",
                Difficulty = DifficultyLevel.Medium,
                TimeLimitMs = 3000,
                MemoryLimitMb = 512
            }
        };

        await context.Problems.AddRangeAsync(problems);
        await context.SaveChangesAsync();
        return problems.ToList();
    }

    private static async Task SeedTestCasesAsync(ApplicationDbContext context, List<Problem> problems)
    {
        if (await context.TestCases.AnyAsync())
            return;

        var testCases = new List<TestCase>();

        // Test cases for Two Sum (problem 0)
        if (problems.Count > 0)
        {
            testCases.AddRange(new[]
            {
                new TestCase { ProblemId = problems[0].Id, Input = "[2,7,11,15]\n9", ExpectedOutput = "[0,1]", IsHidden = false, Order = 1 },
                new TestCase { ProblemId = problems[0].Id, Input = "[3,2,4]\n6", ExpectedOutput = "[1,2]", IsHidden = false, Order = 2 },
                new TestCase { ProblemId = problems[0].Id, Input = "[3,3]\n6", ExpectedOutput = "[0,1]", IsHidden = true, Order = 3 }
            });
        }

        // Test cases for Valid Parentheses (problem 4)
        if (problems.Count > 4)
        {
            testCases.AddRange(new[]
            {
                new TestCase { ProblemId = problems[4].Id, Input = "()", ExpectedOutput = "true", IsHidden = false, Order = 1 },
                new TestCase { ProblemId = problems[4].Id, Input = "()[]{}", ExpectedOutput = "true", IsHidden = false, Order = 2 },
                new TestCase { ProblemId = problems[4].Id, Input = "(]", ExpectedOutput = "false", IsHidden = false, Order = 3 },
                new TestCase { ProblemId = problems[4].Id, Input = "([)]", ExpectedOutput = "false", IsHidden = true, Order = 4 }
            });
        }

        // Test cases for Best Time to Buy and Sell Stock (problem 5)
        if (problems.Count > 5)
        {
            testCases.AddRange(new[]
            {
                new TestCase { ProblemId = problems[5].Id, Input = "[7,1,5,3,6,4]", ExpectedOutput = "5", IsHidden = false, Order = 1 },
                new TestCase { ProblemId = problems[5].Id, Input = "[7,6,4,3,1]", ExpectedOutput = "0", IsHidden = false, Order = 2 },
                new TestCase { ProblemId = problems[5].Id, Input = "[1,2,3,4,5]", ExpectedOutput = "4", IsHidden = true, Order = 3 }
            });
        }

        await context.TestCases.AddRangeAsync(testCases);
    }

    private static async Task SeedProblemTopicsAsync(ApplicationDbContext context, List<Problem> problems)
    {
        if (await context.ProblemTopics.AnyAsync())
            return;

        var topics = await context.Topics.ToListAsync();
        if (topics.Count == 0 || problems.Count == 0)
            return;

        var problemTopics = new List<ProblemTopic>();

        // Two Sum -> Arrays
        if (problems.Count > 0)
        {
            var arraysTopic = topics.FirstOrDefault(t => t.Name == "Arrays");
            if (arraysTopic != null)
                problemTopics.Add(new ProblemTopic { ProblemId = problems[0].Id, TopicId = arraysTopic.Id });
        }

        // Valid Parentheses -> Strings
        if (problems.Count > 4)
        {
            var stringsTopic = topics.FirstOrDefault(t => t.Name == "Strings");
            if (stringsTopic != null)
                problemTopics.Add(new ProblemTopic { ProblemId = problems[4].Id, TopicId = stringsTopic.Id });
        }

        // Best Time to Buy and Sell Stock -> Arrays, Dynamic Programming
        if (problems.Count > 5)
        {
            var arraysTopic = topics.FirstOrDefault(t => t.Name == "Arrays");
            var dpTopic = topics.FirstOrDefault(t => t.Name == "Dynamic Programming");
            if (arraysTopic != null)
                problemTopics.Add(new ProblemTopic { ProblemId = problems[5].Id, TopicId = arraysTopic.Id });
            if (dpTopic != null)
                problemTopics.Add(new ProblemTopic { ProblemId = problems[5].Id, TopicId = dpTopic.Id });
        }

        // Longest Substring -> Strings
        if (problems.Count > 2)
        {
            var stringsTopic = topics.FirstOrDefault(t => t.Name == "Strings");
            if (stringsTopic != null)
                problemTopics.Add(new ProblemTopic { ProblemId = problems[2].Id, TopicId = stringsTopic.Id });
        }

        // 3Sum -> Arrays
        if (problems.Count > 7)
        {
            var arraysTopic = topics.FirstOrDefault(t => t.Name == "Arrays");
            if (arraysTopic != null)
                problemTopics.Add(new ProblemTopic { ProblemId = problems[7].Id, TopicId = arraysTopic.Id });
        }

        await context.ProblemTopics.AddRangeAsync(problemTopics);
    }

    private static async Task SeedSubmissionsAsync(ApplicationDbContext context, List<Problem> problems, List<ApplicationUser> users)
    {
        if (await context.Submissions.AnyAsync())
            return;

        if (problems.Count == 0 || users.Count == 0)
            return;

        var submissions = new List<Submission>();

        // Create some accepted submissions
        if (problems.Count > 0 && users.Count > 0)
        {
            submissions.Add(new Submission
            {
                ProblemId = problems[0].Id,
                UserId = users[0].Id,
                Code = "public int[] TwoSum(int[] nums, int target) {\n    var map = new Dictionary<int, int>();\n    for (int i = 0; i < nums.Length; i++) {\n        int complement = target - nums[i];\n        if (map.ContainsKey(complement)) {\n            return new int[] { map[complement], i };\n        }\n        map[nums[i]] = i;\n    }\n    return new int[0];\n}",
                Language = ProgrammingLanguage.CSharp,
                Status = SubmissionStatus.Accepted,
                ExecutionTimeMs = 150,
                MemoryUsedMb = 45,
                TestCasesPassed = 3,
                TotalTestCases = 3
            });
        }

        if (problems.Count > 4 && users.Count > 1)
        {
            submissions.Add(new Submission
            {
                ProblemId = problems[4].Id,
                UserId = users[1].Id,
                Code = "public bool IsValid(string s) {\n    var stack = new Stack<char>();\n    foreach (char c in s) {\n        if (c == '(' || c == '[' || c == '{') {\n            stack.Push(c);\n        } else if (stack.Count == 0) {\n            return false;\n        } else {\n            char top = stack.Pop();\n            if ((c == ')' && top != '(') || (c == ']' && top != '[') || (c == '}' && top != '{')) {\n                return false;\n            }\n        }\n    }\n    return stack.Count == 0;\n}",
                Language = ProgrammingLanguage.CSharp,
                Status = SubmissionStatus.Accepted,
                ExecutionTimeMs = 80,
                MemoryUsedMb = 38,
                TestCasesPassed = 4,
                TotalTestCases = 4
            });
        }

        // Create some failed submissions
        if (problems.Count > 0 && users.Count > 2)
        {
            submissions.Add(new Submission
            {
                ProblemId = problems[0].Id,
                UserId = users[2].Id,
                Code = "public int[] TwoSum(int[] nums, int target) {\n    for (int i = 0; i < nums.Length; i++) {\n        for (int j = i + 1; j < nums.Length; j++) {\n            if (nums[i] + nums[j] == target) {\n                return new int[] { i, j };\n            }\n        }\n    }\n    return new int[0];\n}",
                Language = ProgrammingLanguage.CSharp,
                Status = SubmissionStatus.TimeLimitExceeded,
                ExecutionTimeMs = 2500,
                MemoryUsedMb = 50,
                TestCasesPassed = 2,
                TotalTestCases = 3,
                ErrorMessage = "Time limit exceeded"
            });
        }

        if (problems.Count > 5 && users.Count > 0)
        {
            submissions.Add(new Submission
            {
                ProblemId = problems[5].Id,
                UserId = users[0].Id,
                Code = "public int MaxProfit(int[] prices) {\n    int maxProfit = 0;\n    for (int i = 0; i < prices.Length; i++) {\n        for (int j = i + 1; j < prices.Length; j++) {\n            int profit = prices[j] - prices[i];\n            if (profit > maxProfit) {\n                maxProfit = profit;\n            }\n        }\n    }\n    return maxProfit;\n}",
                Language = ProgrammingLanguage.CSharp,
                Status = SubmissionStatus.Accepted,
                ExecutionTimeMs = 200,
                MemoryUsedMb = 42,
                TestCasesPassed = 3,
                TotalTestCases = 3
            });
        }

        await context.Submissions.AddRangeAsync(submissions);

        // Update problem statistics
        foreach (var problem in problems)
        {
            var problemSubmissions = submissions.Where(s => s.ProblemId == problem.Id).ToList();
            problem.TotalSubmissions = problemSubmissions.Count;
            problem.AcceptedSubmissions = problemSubmissions.Count(s => s.Status == SubmissionStatus.Accepted);
        }
    }

    private static async Task<List<Solution>> SeedSolutionsAsync(ApplicationDbContext context, List<Problem> problems, List<ApplicationUser> users)
    {
        if (await context.Solutions.AnyAsync())
            return await context.Solutions.ToListAsync();

        if (problems.Count == 0 || users.Count == 0)
            return new List<Solution>();

        var solutions = new List<Solution>();

        if (problems.Count > 0 && users.Count > 0)
        {
            solutions.Add(new Solution
            {
                ProblemId = problems[0].Id,
                UserId = users[0].Id,
                Title = "Efficient Hash Map Solution",
                Content = "This solution uses a hash map to store the complement of each number. We iterate through the array once, checking if the complement exists in the map. Time complexity: O(n), Space complexity: O(n).",
                Code = "public int[] TwoSum(int[] nums, int target) {\n    var map = new Dictionary<int, int>();\n    for (int i = 0; i < nums.Length; i++) {\n        int complement = target - nums[i];\n        if (map.ContainsKey(complement)) {\n            return new int[] { map[complement], i };\n        }\n        map[nums[i]] = i;\n    }\n    return new int[0];\n}",
                LikesCount = 0,
                CommentsCount = 0
            });
        }

        if (problems.Count > 4 && users.Count > 1)
        {
            solutions.Add(new Solution
            {
                ProblemId = problems[4].Id,
                UserId = users[1].Id,
                Title = "Stack-Based Approach",
                Content = "Use a stack to keep track of opening brackets. When we encounter a closing bracket, we check if it matches the most recent opening bracket. Time complexity: O(n), Space complexity: O(n).",
                Code = "public bool IsValid(string s) {\n    var stack = new Stack<char>();\n    foreach (char c in s) {\n        if (c == '(' || c == '[' || c == '{') {\n            stack.Push(c);\n        } else if (stack.Count == 0) {\n            return false;\n        } else {\n            char top = stack.Pop();\n            if ((c == ')' && top != '(') || (c == ']' && top != '[') || (c == '}' && top != '{')) {\n                return false;\n            }\n        }\n    }\n    return stack.Count == 0;\n}",
                LikesCount = 0,
                CommentsCount = 0
            });
        }

        if (problems.Count > 5 && users.Count > 2)
        {
            solutions.Add(new Solution
            {
                ProblemId = problems[5].Id,
                UserId = users[2].Id,
                Title = "One Pass Solution",
                Content = "We only need to track the minimum price seen so far and the maximum profit. This can be done in a single pass. Time complexity: O(n), Space complexity: O(1).",
                Code = "public int MaxProfit(int[] prices) {\n    int minPrice = int.MaxValue;\n    int maxProfit = 0;\n    foreach (int price in prices) {\n        if (price < minPrice) {\n            minPrice = price;\n        } else if (price - minPrice > maxProfit) {\n            maxProfit = price - minPrice;\n        }\n    }\n    return maxProfit;\n}",
                LikesCount = 0,
                CommentsCount = 0
            });
        }

        await context.Solutions.AddRangeAsync(solutions);
        await context.SaveChangesAsync();
        return solutions;
    }

    private static async Task SeedSolutionLikesAsync(ApplicationDbContext context, List<Solution> solutions, List<ApplicationUser> users)
    {
        if (await context.SolutionLikes.AnyAsync())
            return;

        if (solutions.Count == 0 || users.Count == 0)
            return;

        var likes = new List<SolutionLike>();

        // Like first solution with multiple users
        if (solutions.Count > 0)
        {
            if (users.Count > 1)
                likes.Add(new SolutionLike { SolutionId = solutions[0].Id, UserId = users[1].Id });
            if (users.Count > 2)
                likes.Add(new SolutionLike { SolutionId = solutions[0].Id, UserId = users[2].Id });
            if (users.Count > 3)
                likes.Add(new SolutionLike { SolutionId = solutions[0].Id, UserId = users[3].Id });

            solutions[0].LikesCount = likes.Count(l => l.SolutionId == solutions[0].Id);
        }

        // Like second solution
        if (solutions.Count > 1)
        {
            if (users.Count > 0)
                likes.Add(new SolutionLike { SolutionId = solutions[1].Id, UserId = users[0].Id });
            if (users.Count > 2)
                likes.Add(new SolutionLike { SolutionId = solutions[1].Id, UserId = users[2].Id });

            solutions[1].LikesCount = likes.Count(l => l.SolutionId == solutions[1].Id);
        }

        // Like third solution
        if (solutions.Count > 2)
        {
            if (users.Count > 0)
                likes.Add(new SolutionLike { SolutionId = solutions[2].Id, UserId = users[0].Id });
            if (users.Count > 1)
                likes.Add(new SolutionLike { SolutionId = solutions[2].Id, UserId = users[1].Id });

            solutions[2].LikesCount = likes.Count(l => l.SolutionId == solutions[2].Id);
        }

        await context.SolutionLikes.AddRangeAsync(likes);
    }

    private static async Task<List<Comment>> SeedCommentsAsync(ApplicationDbContext context, List<Solution> solutions, List<ApplicationUser> users)
    {
        if (await context.Comments.AnyAsync())
            return await context.Comments.ToListAsync();

        if (solutions.Count == 0 || users.Count == 0)
            return new List<Comment>();

        var comments = new List<Comment>();

        if (solutions.Count > 0)
        {
            if (users.Count > 1)
            {
                comments.Add(new Comment
                {
                    SolutionId = solutions[0].Id,
                    UserId = users[1].Id,
                    Content = "Great solution! Very clean and efficient."
                });
            }

            if (users.Count > 2)
            {
                comments.Add(new Comment
                {
                    SolutionId = solutions[0].Id,
                    UserId = users[2].Id,
                    Content = "Thanks for sharing! This helped me understand the hash map approach better."
                });
            }

            solutions[0].CommentsCount = comments.Count(c => c.SolutionId == solutions[0].Id);
        }

        if (solutions.Count > 1 && users.Count > 0)
        {
            comments.Add(new Comment
            {
                SolutionId = solutions[1].Id,
                UserId = users[0].Id,
                Content = "Nice use of stack! This is exactly what I was looking for."
            });

            solutions[1].CommentsCount = comments.Count(c => c.SolutionId == solutions[1].Id);
        }

        await context.Comments.AddRangeAsync(comments);
        await context.SaveChangesAsync();
        return comments;
    }

    private static async Task SeedCommentLikesAsync(ApplicationDbContext context, List<Comment> comments, List<ApplicationUser> users)
    {
        if (await context.CommentLikes.AnyAsync())
            return;

        if (comments.Count == 0 || users.Count == 0)
            return;

        var commentLikes = new List<CommentLike>();

        // Like first comment
        if (comments.Count > 0)
        {
            if (users.Count > 0)
                commentLikes.Add(new CommentLike { CommentId = comments[0].Id, UserId = users[0].Id });
            if (users.Count > 3)
                commentLikes.Add(new CommentLike { CommentId = comments[0].Id, UserId = users[3].Id });
        }

        // Like second comment
        if (comments.Count > 1)
        {
            if (users.Count > 1)
                commentLikes.Add(new CommentLike { CommentId = comments[1].Id, UserId = users[1].Id });
        }

        // Like third comment
        if (comments.Count > 2)
        {
            if (users.Count > 2)
                commentLikes.Add(new CommentLike { CommentId = comments[2].Id, UserId = users[2].Id });
        }

        await context.CommentLikes.AddRangeAsync(commentLikes);
    }

    private static async Task SeedProblemLikesAsync(ApplicationDbContext context, List<Problem> problems, List<ApplicationUser> users)
    {
        if (await context.ProblemLikes.AnyAsync())
            return;

        if (problems.Count == 0 || users.Count == 0)
            return;

        var problemLikes = new List<ProblemLike>();

        // Like some problems
        if (problems.Count > 0)
        {
            if (users.Count > 0)
                problemLikes.Add(new ProblemLike { ProblemId = problems[0].Id, UserId = users[0].Id });
            if (users.Count > 1)
                problemLikes.Add(new ProblemLike { ProblemId = problems[0].Id, UserId = users[1].Id });
        }

        if (problems.Count > 4)
        {
            if (users.Count > 0)
                problemLikes.Add(new ProblemLike { ProblemId = problems[4].Id, UserId = users[0].Id });
            if (users.Count > 2)
                problemLikes.Add(new ProblemLike { ProblemId = problems[4].Id, UserId = users[2].Id });
        }

        if (problems.Count > 5)
        {
            if (users.Count > 1)
                problemLikes.Add(new ProblemLike { ProblemId = problems[5].Id, UserId = users[1].Id });
            if (users.Count > 3)
                problemLikes.Add(new ProblemLike { ProblemId = problems[5].Id, UserId = users[3].Id });
        }

        await context.ProblemLikes.AddRangeAsync(problemLikes);
    }
}
