using Identification;

namespace Rolling
{
    public sealed class Roles : IEquatable<Roles>
    {
        private Dictionary<string, IIdentifiable?> roles;

        public IEnumerable<string> RoleNames => roles.Keys;

        public IEnumerable<IIdentifiable> Matchers => roles
            .Values
            .Where(v => v is not null)
            .ToList()!;

        public bool AllMatched => !roles.Values.Any(v => v is null);

        public IEnumerable<string> Unmatched => roles
            .Keys
            .Where(key => roles[key] is null)
            .ToList();

        public Roles(ISet<string> roleNames)
        {
            roles = new Dictionary<string, IIdentifiable?>();

            if (!roleNames.Contains(Descriptor.MainRole))
                roles.Add(Descriptor.MainRole, null);

            foreach (var item in roleNames)
            {
                roles.Add(item, null);
            }
        }

        public Roles Match(string role, IIdentifiable identifiable)
        {
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException(nameof(role));

            if (!roles.ContainsKey(role))
                throw new ArgumentException(nameof(role));

            roles[role] = identifiable;
            return this;
        }

        public IIdentifiable? Get(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException(nameof(role));

            if (!roles.ContainsKey(role))
                throw new ArgumentException($"{role} is not a matched role.");

            return roles[role];
        }

        public T Get<T>(string role)
            where T : class, IIdentifiable
        {
            var elem = Get(role) as T;
            if (elem == null)
                throw new ArgumentException($"{role} required is not a valid instance of {typeof(T).Name}");

            return elem;
        }

        public bool HasMatched(string role) =>
            roles.ContainsKey(role) && !(roles[role] is null);

        public bool Matched(IIdentifiable identifiable) =>
            roles.Values.Contains(identifiable);

        public Roles Copy()
        {
            var copy = Empty;
            copy.roles = roles.ToDictionary(x => x.Key, x => x.Value);
            return copy;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (!(obj is Roles)) return false;

            return Equals((Roles)obj);
        }

        public bool Equals(Roles? other)
        {
            if (other is null) return false;
            if (roles.Count != other.roles.Count) return false;

            foreach (var role in roles.Keys)
            {
                if (!other.roles.ContainsKey(role)) return false;
                if (roles[role] != other.roles[role]) return false;
            }

            return true;
        }

        public override int GetHashCode() =>
            string.Join(
                "",
                roles.Keys
                    .OrderBy(x => x)
                    .Select(x => $"{x}{roles[x]}"))
                .GetHashCode();

        public override string ToString() =>
            string.Join(',', roles.Keys.Select(role => $"{roles[role]} as {role}"));

        public static Roles Empty => new Roles(new HashSet<string>());
    }
}