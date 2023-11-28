namespace JokesWebApp.Models
{
    public class Joke
    {
        public int Id { get; set; }
        public string JokeQuestion { get; set; }
        public string JokeAnswer { get; set; }

        public Joke() { }   
    }

    public class account
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
