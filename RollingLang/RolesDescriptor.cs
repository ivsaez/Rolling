using Agents;
using Instanciation;
using Items;
using ItemsLang;
using Languager;
using Mapping;
using MappingLang;
using Rolling;
using Saver;
using System.Data;

namespace RollingLang
{
    public class RolesDescriptor : Descriptor<Roles>
    {
        public RolesDescriptor(Roles elem)
            : base(elem)
        {
        }

        public override string Name => elem.ToString();

        public override string Description => elem.ToString();

        public IDictionary<string, string> Map<A, I, M>(Existents<A, I, M> existents)
            where A : IAgent, ISavable, ICloneable
            where I : IItem, ISavable, ICloneable
            where M : IMapped, ISavable, ICloneable
        {
            var identifiers = elem.Matchers.Select(i => i.Id).ToArray();
            var agents = existents.GetExistingAgents(identifiers);
            var items = existents.GetExistingItems(identifiers);
            var mappeds = existents.GetExistingMappeds(identifiers);

            var rolenaming = new Dictionary<string, string>();
            foreach (var agent in agents)
                rolenaming.Add(elem.MatchedRole(agent)!, agent.Name);

            foreach (var item in items)
                rolenaming.Add(elem.MatchedRole(item)!, new ItemDescriptor(item).ArticledName(true));

            foreach (var mapped in mappeds)
                rolenaming.Add(elem.MatchedRole(mapped)!, new MappedDescriptor(mapped).ArticledName);

            return rolenaming;
        }
    }
}