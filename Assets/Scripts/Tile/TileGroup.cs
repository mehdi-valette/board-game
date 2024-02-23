
using System.Collections.Generic;
using Unity.VisualScripting;

public class TileGroup
{
    private HashSet<Tile> anchors = new HashSet<Tile>();
    private HashSet<Tile> members = new HashSet<Tile>();

    public TileGroup(HashSet<Tile> members)
    {
        foreach (var member in members)
        {
            if (member.GetTileType() == PieceType.Temple)
            {
                anchors.Add(member);
            }
            else
            {
                members.Add(member);
            }
        }

        NotifyMembers();
    }

    public void AddMember(Tile member)
    {
        if (member.GetTileType() == PieceType.Temple)
        {
            anchors.Add(member);
        }
        else
        {
            members.Add(member);
        }

        member.SetGroup(this);
    }

    public void RemoveMember(Tile removedMember)
    {
        if (removedMember.GetTileType() == PieceType.Temple)
        {
            anchors.Remove(removedMember);
        }
        else
        {
            members.Remove(removedMember);
        }

        HashSet<TileGroup> newGroups = new HashSet<TileGroup>();
        HashSet<Tile> visited = new HashSet<Tile>();

        // each anchor may start a new group, unless they're already part of a group
        foreach (var anchor in anchors)
        {
            if (visited.Contains(anchor))
            {
                continue;
            }

            var newlyVisited = GetGroupFromTile(anchor);
            visited.AddRange(newlyVisited);
            newGroups.Add(new TileGroup(newlyVisited));
        }

        // reset the tiles that weren't visited (not connected to an anchor)
        foreach (var member in members)
        {
            if(!visited.Contains(member))
            {
                member.ClearTile();
            }
        }
        // if there's only one group (or for some reason 0), no action needed
        if (newGroups.Count < 2)
        {
            return;
        }

        //assign each member to its new group
        foreach (var group in newGroups)
        {
            foreach (var member in group.members)
            {
                member.SetGroup(group);
            }
        }
    }

    public void MergeOtherGroup(TileGroup group)
    {
        anchors.AddRange(group.anchors);
        members.AddRange(group.members);

        NotifyMembers();
    }

    public void MergeIntoOtherGroup(TileGroup group)
    {
        group.MergeOtherGroup(this);
    }

    private HashSet<Tile> GetGroupFromTile(Tile root)
    {
        Stack<Tile> toVisit = new();
        HashSet<Tile> visited = new();

        toVisit.Push(root);

        // TODO: fix the algorithm, it doesn't see all the tiles it should visit
        while (toVisit.Count > 0)
        {
            var currentTile = toVisit.Pop();
            if (visited.Contains(currentTile)) continue;
            foreach (var neighbour in root.GetNeighbours())
            {
                if (Tile.IsFriendlyNeighbour(neighbour, root.GetTileCamp()) && !visited.Contains(neighbour))
                {
                    toVisit.Push(neighbour);
                }
            }

            visited.Add(currentTile);
        }

        return visited;
    }

    // notify the tiles that they are now part of this group
    private void NotifyMembers()
    {
        foreach (var anchor in anchors)
        {
            anchor.SetGroup(this);
        }

        foreach (var member in members)
        {
            member.SetGroup(this);
        }
    }
}