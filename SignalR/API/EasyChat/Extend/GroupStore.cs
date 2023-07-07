namespace EasyChat.Extend
{
    public static class GroupStore
    {
        public static Dictionary<string, List<string>> Groups = new Dictionary<string, List<string>>();
        public static void Add(string groupname, string Id)
        {
            if (Groups.ContainsKey(groupname))
            {
                if (Groups.TryGetValue(groupname, out var values))
                {
                    if (values.Contains(Id))
                        return;
                    values.Add(Id);
                }
                else
                {
                    throw new Exception("Add group Error");
                }
            }
            else
            {
                var newvalues = new List<string>() { Id };
                Groups.Add(groupname, newvalues);
            }
        }
        public static void Remove(string groupname, string Id)
        {
            if (Groups.ContainsKey(groupname))
            {
                if (Groups.TryGetValue(groupname, out var values))
                {
                    if (!values.Contains(Id))
                        return;
                    values.Remove(Id);
                    if (!(values.Count > 0))
                        Groups.Remove(groupname);
                }
                else
                {
                    throw new Exception("Remove group Error");
                }
            }
        }
        /// <summary>
        /// 连接断开时删除
        /// </summary>
        /// <param name="Id"></param>
        public static void UnConnection(string Id)
        {
            Groups.Where(x => x.Value.Contains(Id)).AsParallel().ForAll(x => x.Value.Remove(Id));
        }
    }
}
