using Identification;

namespace Rolling.Test
{
    public class DescriptorShould
    {
        private Identifiable Main;

        public DescriptorShould()
        {
            Main = new Identifiable("Main");
        }

        [Fact]
        public void ReturnValidPermutationsForEmptyDescriptor()
        {
            var descriptor = Descriptor.Empty;

            var permutations = descriptor.GetPermutations(Main, new HashSet<IIdentifiable>());

            Assert.NotNull(permutations);
            Assert.True(permutations.Any);
            Assert.Single(permutations.Roles);

            var roles = permutations.Roles.First();

            Assert.NotNull(roles);
            Assert.True(roles.AllMatched);
        }

        [Fact]
        public void ReturnSinglePermutationWithOneName()
        {
            var descriptor = Descriptor.New("single");

            var permutations = descriptor.GetPermutations(Main, new HashSet<IIdentifiable>
            {
                new Identifiable("one")
            });

            Assert.NotNull(permutations);
            Assert.True(permutations.Any);
            Assert.Single(permutations.Roles);

            var roles = permutations.Roles.First();

            Assert.NotNull(roles);
            Assert.True(roles.AllMatched);

            Assert.True(roles.HasMatched("single"));
        }

        [Fact]
        public void ReturnEmptyPermutationsWhenInvalidInput()
        {
            var descriptor = Descriptor.New("first", "second");

            var permutations = descriptor.GetPermutations(Main, new HashSet<IIdentifiable>
            {
                new Identifiable("one")
            });

            Assert.NotNull(permutations);
            Assert.False(permutations.Any);
        }

        [Fact]
        public void ReturnTwoPermutationWithTwoNames()
        {
            var names = new string[]
            {
                "first",
                "second",
            };

            var descriptor = Descriptor.New(names);

            var identifiables = new HashSet<IIdentifiable>
            {
                new Identifiable("one"),
                new Identifiable("two")
            };

            var permutations = descriptor.GetPermutations(Main, identifiables);

            Assert.NotNull(permutations);
            Assert.True(permutations.Any);
            Assert.Equal(2, permutations.Roles.Count());

            foreach (var roles in permutations.Roles)
            {
                foreach (var name in names)
                {
                    Assert.True(roles.HasMatched(name));
                }
            }
        }

        [Fact]
        public void ReturnSixPermutationsWithThreeNames()
        {
            var names = new string[]
            {
                "first",
                "second",
                "third"
            };

            var descriptor = Descriptor.New(names);

            var identifiables = new HashSet<IIdentifiable>
            {
                new Identifiable("one"),
                new Identifiable("two"),
                new Identifiable("three")
            };

            var permutations = descriptor.GetPermutations(Main, identifiables);

            Assert.NotNull(permutations);
            Assert.True(permutations.Any);
            Assert.Equal(6, permutations.Roles.Count());

            foreach (var roles in permutations.Roles)
            {
                foreach (var name in names)
                {
                    Assert.True(roles.HasMatched(name));
                }
            }
        }

        [Fact]
        public void ReturnTwentyFourPermutationsWithFourNames()
        {
            var names = new string[]
            {
                "first",
                "second",
                "third",
                "fourth"
            };

            var descriptor = Descriptor.New(names);

            var identifiables = new HashSet<IIdentifiable>
            {
                new Identifiable("one"),
                new Identifiable("two"),
                new Identifiable("three"),
                new Identifiable("four")
            };

            var permutations = descriptor.GetPermutations(Main, identifiables);

            Assert.NotNull(permutations);
            Assert.True(permutations.Any);
            Assert.Equal(24, permutations.Roles.Count());

            foreach (var roles in permutations.Roles)
            {
                foreach (var name in names)
                {
                    Assert.True(roles.HasMatched(name));
                }
            }
        }

        [Fact]
        public void LotsOfElementsTest()
        {
            var names = new string[]
            {
                "first",
                "second",
                "third",
                "fourth",
                "fifth"
            };

            var descriptor = Descriptor.New(names);

            var identifiables = new HashSet<IIdentifiable>();
            for (int i = 0; i < 20; i++)
            {
                identifiables.Add(new Identifiable("identifiable" + i.ToString()));
            }

            var permutations = descriptor.GetPermutations(Main, identifiables);

            Assert.NotNull(permutations);
            Assert.True(permutations.Any);
        }
    }

    internal class Identifiable : IIdentifiable
    {
        public string Id { get; }

        public Identifiable(string id)
        {
            Id = id;
        }

        public override string ToString() => Id;
    }
}