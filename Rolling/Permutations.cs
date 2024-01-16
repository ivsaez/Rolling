namespace Rolling
{
    public sealed class Permutations
    {
        private List<Roles> roles;

        public bool Any => roles.Any();

        public bool Empty => !Any;

        public IEnumerable<Roles> Roles => roles;

        public Permutations()
        {
            roles = new List<Roles>();
        }

        public void Add(Roles roles)
        {
            if(roles is null)
                throw new ArgumentNullException(nameof(roles));

            if(!roles.AllMatched)
                throw new ArgumentException("Roles must be matched to be added into a permutation.");

            this.roles.Add(roles);
        }

        public Permutations With(Roles roles)
        {
            Add(roles);
            return this;
        }
    }
}
