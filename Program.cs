using System;
using RedditSharp; // reddit API
using System.Linq; // queries
using System.Collections.Generic; // lists
using System.IO; // read/write files

namespace RedditImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> users; // users
            List<string> subs; // subs
            Reddit impReddit = new Reddit(); // account you want to import to            
            Reddit expReddit = new Reddit(); // account you want to export from
            string usr, usr1, pwd, pwd1, subFilePath, userFilePath;
            Console.Clear();

            while (true)
            {
                Console.WriteLine("\nMake a choice:");
                Console.WriteLine("1. Import subreddits and/or users from txt file.");
                Console.WriteLine("2. Export subreddits and users to txt files.");
                Console.WriteLine("3. Import subreddits and/or users from another account.");
                Console.WriteLine("4. Unsuscribe/unfollow subreddits/users from txt file.");
                Console.WriteLine("5. Unsubscribe and/or unfollow all current subreddits and users.");
                Console.WriteLine("6. Display current subreddits and users.");
                Console.WriteLine("\nTo exit, press ctrl + c");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter subreddits file path, or drag file here: ");
                        subFilePath = Console.ReadLine();
                        subs = new List<string>(File.ReadAllLines(subFilePath));
                        Console.WriteLine("Enter users file path: ");
                        userFilePath = Console.ReadLine();
                        users = new List<string>(File.ReadAllLines(userFilePath));
                        Console.WriteLine("Please log in to the account you want to import to.");
                        Console.Write("\nEnter username: ");
                        usr1 = Console.ReadLine();
                        Console.Write("Enter password: ");
                        pwd1 = Console.ReadLine();
                        impReddit.LogIn(usr1, pwd1);
                        Console.WriteLine("\nSubreddits:");
                        Subscribe(impReddit, subs);
                        Console.WriteLine("\nUsers:\n");
                        Follow(impReddit, users);
                        break;

                    case "2":
                        Console.WriteLine("Enter a path to export files to, or drag folder here: ");
                        subFilePath = Console.ReadLine();
                        Console.WriteLine("Please log in to the account you want to export from.");
                        Console.Write("\nEnter username: ");
                        usr1 = Console.ReadLine();
                        Console.Write("Enter password: ");
                        pwd1 = Console.ReadLine();
                        impReddit.LogIn(usr1, pwd1);
                        ShowSubsUsers(impReddit, out subs, out users);
                        File.WriteAllLines(subFilePath + impReddit.User + "_users.txt", users); // export users list to file
                        File.WriteAllLines(subFilePath + impReddit.User + "_subs.txt", subs); // export subs list to file
                        Console.WriteLine("\nSuccessfully exported users and subreddits to txt files.");
                        break;

                    case "3":
                        Console.WriteLine("Please login to the account you want to export from.");
                        Console.Write("Username: ");
                        usr = Console.ReadLine();
                        Console.Write("Password: ");
                        pwd = Console.ReadLine();
                        expReddit.LogIn(usr, pwd);
                        Console.WriteLine($"\n{expReddit.User} is currently subsribed to the following subreddits:\n");
                        ShowSubsUsers(expReddit, out subs, out users);
                        Console.WriteLine("Please log in to the account you want to import to.");
                        Console.Write("Username: ");
                        usr1 = Console.ReadLine();
                        Console.Write("Password: ");
                        pwd1 = Console.ReadLine();
                        impReddit.LogIn(usr1, pwd1);
                        Console.WriteLine("\n\nDo you want to subscribe to all the listed subreddits?");
                        string answer1 = Console.ReadLine();
                        answer1 = answer1.ToLower();

                        while (true) // join subs
                        {
                            if (answer1 == "yes")
                            {
                                Subscribe(impReddit, subs);
                                break;
                            }
                            else if (answer1 == "no")
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Please answer with 'yes' or 'no'.");
                                Console.WriteLine("\nDo you want to subscribe to all listed subreddits?");
                                answer1 = Console.ReadLine();
                                answer1 = answer1.ToLower();
                            }
                        }

                        Console.WriteLine("\n\nDo you want to follow all the listed users?");
                        string answer2 = Console.ReadLine();
                        answer2 = answer2.ToLower();

                        while (true) // follow users
                        {
                            if (answer2 == "yes")
                            {
                                Follow(impReddit, users);
                                break;
                            }
                            else if (answer2 == "no")
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Please answer with 'yes' or 'no'.");
                                Console.WriteLine("\nDo you want to follow all listed users?");
                                answer2 = Console.ReadLine();
                                answer2 = answer2.ToLower();
                            }
                        }
                        break;

                    case "4":
                        Console.WriteLine("Enter subreddits file path, or drag file here: ");
                        subFilePath = Console.ReadLine();
                        subs = new List<string>(File.ReadAllLines(subFilePath));
                        Console.WriteLine("Enter users file path: ");
                        userFilePath = Console.ReadLine();
                        users = new List<string>(File.ReadAllLines(userFilePath));
                        Console.WriteLine("Please login.");
                        Console.Write("Enter username: ");
                        usr = Console.ReadLine();
                        Console.Write("Enter password: ");
                        pwd = Console.ReadLine();
                        expReddit.LogIn(usr, pwd);
                        Console.WriteLine($"\nDo you want to unsubscribe {expReddit.User} from all the listed subreddits?");
                        string answer3 = Console.ReadLine();
                        answer3 = answer3.ToLower();

                        while (true) // leave subs
                        {
                            if (answer3 == "yes")
                            {
                                Unsubscribe(expReddit, subs);
                                break;
                            }
                            else if (answer3 == "no")
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Please answer with 'yes' or 'no'.");
                                Console.WriteLine($"Do you want to unsubscribe {expReddit.User} from all the listed subreddits?");
                                answer3 = Console.ReadLine();
                                answer3 = answer3.ToLower();
                            }
                        }
                        Console.WriteLine($"\nDo you want to unfollow all the listed users from {expReddit.User}?");
                        string answer4 = Console.ReadLine();
                        answer4 = answer4.ToLower();
                        while (true) // unfollow users
                        {
                            if (answer4 == "yes")
                            {
                                Unfollow(expReddit, users);
                                break;
                            }
                            else if (answer4 == "no")
                            {
                                Console.WriteLine("Goodbye!");
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Please answer with 'yes' or 'no'.");
                                Console.WriteLine($"Do you want to unfollow all the listed users from {expReddit.User}?");
                                answer4 = Console.ReadLine();
                                answer4 = answer4.ToLower();
                            }
                        }
                        break;
                    case "5":
                        Console.WriteLine("Please login.");
                        Console.Write("Enter username: ");
                        usr = Console.ReadLine();
                        Console.Write("Enter password: ");
                        pwd = Console.ReadLine();
                        expReddit.LogIn(usr, pwd);

                        Console.WriteLine($"\nDo you want to unfollow all users from {expReddit.User}?");
                        string answer5 = Console.ReadLine();
                        answer5 = answer5.ToLower();
                        while (true)
                        {
                            if (answer5 == "yes")
                            {
                                UnfollowAll(expReddit);
                                break;
                            }
                            else if (answer5 == "no")
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Please answer with 'yes' or 'no'.");
                                Console.WriteLine($"Do you want to unfollow all the users from {expReddit.User}?");
                                answer5 = Console.ReadLine();
                                answer5 = answer5.ToLower();
                            }
                        }

                        Console.WriteLine($"\nDo you want to unsubscribe from all subreddits from {expReddit.User}?");
                        string answer6 = Console.ReadLine();
                        answer6 = answer6.ToLower();
                        while (true)
                        {
                            if (answer6 == "yes")
                            {
                                UnsubAll(expReddit);
                                break;
                            }
                            else if (answer6 == "no")
                            {
                                Console.WriteLine("Goodbye!");
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Please answer with 'yes' or 'no'.");
                                Console.WriteLine($"Do you want to unfollow all the users from {expReddit.User}?");
                                answer6 = Console.ReadLine();
                                answer6 = answer6.ToLower();
                            }
                        }

                        break;
                    case "6":
                        Console.WriteLine("Please login.");
                        Console.Write("Enter username: ");
                        usr = Console.ReadLine();
                        Console.Write("Enter password: ");
                        pwd = Console.ReadLine();
                        expReddit.LogIn(usr, pwd);
                        ShowSubsUsers(expReddit, out _, out _);
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("*************************");
                        Console.WriteLine("*  Enter a number 1–6   *");
                        Console.WriteLine("*************************");
                        break;
                }
            }
        }
        public static void Subscribe(Reddit acc, List<string> subs)
        {
            int i = 0;
            foreach (string subreddit in subs)
            {
                acc.GetSubreddit(subreddit).Subscribe();
                i++;
                Console.WriteLine($"Subscribed {acc.User} to {subreddit} ({i} of {subs.Count})");
            }
            Console.WriteLine($"\nSuccessfully subscribed to {subs.Count} subreddits.");
        }
        public static void Unsubscribe(Reddit acc, List<string> subs)
        {
            int i = 0;
            foreach (string subreddit in subs)
            {
                acc.GetSubreddit(subreddit).Unsubscribe();
                i++;
                Console.WriteLine($"Unsubscribed {acc.User} from {subreddit} ({i} of {subs.Count})");
            }
            Console.WriteLine($"\nSuccessfully unsubscribed from {subs.Count} subreddits.");
        }
        public static void UnsubAll(Reddit acc)
        {
            int i = 0;
            int j = acc.User.SubscribedSubreddits.Where(s => !s.Name.StartsWith("/user/")).Count();
            foreach (string subreddit in acc.User.SubscribedSubreddits.Where(s => !s.Name.StartsWith("/user")).Select(s => s.Name))
            {
                acc.GetSubreddit(subreddit).Unsubscribe();
                i++;
                Console.WriteLine($"Unsubscribed {acc.User} from {subreddit} ({i} of {j})");
            }
            Console.WriteLine($"\nSuccessfully unsubscribed from {i} subreddits.");
        }
        public static void UnfollowAll(Reddit acc)
        {
            int i = 0;
            int j = acc.User.SubscribedSubreddits.Where(s => s.Name.StartsWith("/user/")).Count();
            foreach (string subreddit in acc.User.SubscribedSubreddits.Where(s => s.Name.StartsWith("/user")).Select(s => s.Name))
            {
                acc.GetSubreddit("r/u_" + subreddit.Substring(6)).Unsubscribe();
                i++;
                Console.WriteLine($"{acc.User} unfollowed {subreddit.Substring(6)} ({i} of {j})");
            }
            Console.WriteLine($"\nSuccessfully unfollowed {i} users.");
        }
        public static void Follow(Reddit acc, List<string> users)
        {
            int i = 0;
            foreach (string user in users)
            {
                acc.GetSubreddit("r/u_" + user).Subscribe();
                i++;
                Console.WriteLine($"{acc.User} followed {user} ({i} of {users.Count})");
            }
            Console.WriteLine($"\nSuccessfully followed {users.Count} users.");
        }
        public static void Unfollow(Reddit acc, List<string> users)
        {
            int i = 0;
            foreach (string user in users)
            {
                acc.GetSubreddit("r/u_" + user).Unsubscribe();
                i++;
                Console.WriteLine($"{acc.User} unfollowed {user} ({i} of {users.Count})");
            }
            Console.WriteLine($"\nSuccessfully unfollowed {users.Count} users.");
        }
        public static void ShowSubsUsers(Reddit acc, out List<string> subs, out List<string> users)
        {
            users = new List<string>();
            subs = new List<string>();
            Console.WriteLine($"\n{acc.User} is currently subscribed to the following subreddits:\n");
            foreach (string subreddit in acc.User.SubscribedSubreddits.Where(s => !s.Name.StartsWith("/user/")).Select(s => s.Name))
            {
                subs.Add(subreddit);
                Console.WriteLine(subreddit);
            }
            Console.WriteLine($"\n{acc.User} is currently following to the following users:\n");
            foreach (string subreddit in acc.User.SubscribedSubreddits.Where(s => s.Name.StartsWith("/user/")).Select(s => s.Name))
            {
                users.Add(subreddit.Substring(6));
                Console.WriteLine(subreddit.Substring(6));
            }
            Console.WriteLine($"\nShowing {subs.Count} subreddits & {users.Count} users.\n");
        }

    }
}
