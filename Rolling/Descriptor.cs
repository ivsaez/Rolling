using Identification;

namespace Rolling
{
    public class Descriptor
    {
        public ISet<string> Names { get; }

        public bool IsEmpty => !Names.Any();

        public static Descriptor Empty => new Descriptor();

        public static Descriptor New(params string[] roles)
        {
            checkMultipleRoles(roles);

            return new Descriptor(roles);
        }

        public bool IsMatched(Roles roles)
        {
            if (IsEmpty) return roles.IsEmpty;

            return Names.All(name => roles.Has(name));
        }

        public Permutations GetPermutations(ISet<IIdentifiable> identifiables)
        {
            if (IsEmpty)
                return new Permutations()
                    .With(Roles.Empty);

            var result = new Permutations();

            if (identifiables.Count < Names.Count) return result;

            var identifiableCombinations = calculateIdentifiableCombinations(identifiables);

            foreach (var combination in identifiableCombinations)
            {
                foreach (var identifiable in combination)
                {
                    var roles = new Roles(Names);

                    roles.Match(Names.First(), identifiable);
                    var remainingNames = roles.Unmatched;
                    var remainingIdentifiables = combination.Where(x => !roles.Matched(x));

                    fillRemainingPermutations(result, roles, remainingNames, remainingIdentifiables);
                }
            }

            return result;
        }

        protected Descriptor(params string[] roles)
        {
            Names = new HashSet<string>(roles);
        }

        private static void checkValidRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException(nameof(role));
        }

        private static void checkMultipleRoles(IEnumerable<string> roles)
        {
            foreach (var role in roles)
                checkValidRole(role);
        }

        private IEnumerable<IEnumerable<IIdentifiable>> calculateIdentifiableCombinations(ISet<IIdentifiable> identifiables)
        {
            int size = identifiables.Count;
            var oneElemSequences = identifiables.Select(x => new[] { x }).ToList();

            var result = new List<List<IIdentifiable>>
            {
                new List<IIdentifiable>()
            };

            foreach (var oneElemSequence in oneElemSequences)
            {
                int length = result.Count;

                for (int i = 0; i < length; i++)
                {
                    if (result[i].Count >= size)
                        continue;

                    result.Add(result[i].Concat(oneElemSequence).ToList());
                }
            }

            return result.Where(x => x.Count == size);
        }

        private void fillRemainingPermutations(
            Permutations permutations,
            Roles roles, 
            IEnumerable<string> secondarys, 
            IEnumerable<IIdentifiable> identifiables)
        {
            if (!secondarys.Any())
            {
                permutations.Add(roles);
                return;
            }

            var copyRoles = roles.Copy();

            foreach (var identifiable in identifiables)
            {
                copyRoles.Match(secondarys.First(), identifiable);
                var remainingSecondarys =copyRoles.Unmatched;
                var remainingIdentifiables = identifiables.Where(x => !copyRoles.Matched(x));

                fillRemainingPermutations(permutations, copyRoles, remainingSecondarys, remainingIdentifiables);
            }
        }
    }
}
