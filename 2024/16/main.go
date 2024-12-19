package main

import (
	"bufio"
	"fmt"
	"math"
	"os"
	"slices"
)

func main() {
	// Open file
	file, err := os.Open("input.txt")
	if err != nil {
		panic(err)
	}
	defer file.Close()

	// Parse the input
	start, end := Vec4{}, Vec2{}
	maze := make([][]rune, 0, 1000)
	y := 0
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := scanner.Text()
		if len(line) == 0 {
			continue
		}

		row := make([]rune, len(line))
		for x, c := range line {
			row[x] = c
			if c == 'S' {
				start.PosX, start.PosY = x, y
			} else if c == 'E' {
				end.X, end.Y = x, y
			}
		}
		maze = append(maze, row)
		y++
	}

	start.DirX, start.DirY = 1, 0
	directions := []Vec2{{0, -1}, {1, 0}, {0, 1}, {-1, 0}}
	queue := make([]Node, 0, 1000)
	visited := make(map[Vec4]*Node) // Visited should compare position and direction
	queue = append(queue, Node{
		Loc:    start,
		Parent: make([]*Node, 0, 4),
	})
	minScore := math.MaxInt
	paths := make([]Node, 0, 100)
	for {
		if len(queue) == 0 {
			break
		}
		var curr Node
		minCost, idx := math.MaxInt, 0
		for i := range queue {
			if minCost > queue[i].Cost {
				minCost = queue[i].Cost
				idx = i
			}
		}
		curr = queue[idx]
		queue = slices.Delete(queue, idx, idx+1)
		if curr.Loc.PosX == end.X && curr.Loc.PosY == end.Y {
			if curr.Cost < minScore {
				minScore = curr.Cost
				paths = paths[:0]
			}
			if curr.Cost == minScore {
				paths = append(paths, curr)
			}
			continue
		}
		visited[curr.Loc] = &curr

		for i := range directions {
			dir := directions[i]
			if dir.X == -curr.Loc.DirX && dir.Y == -curr.Loc.DirY {
				continue // 180 deg turn - skip
			}
			cost := 1
			if dir.X != curr.Loc.DirX || dir.Y != curr.Loc.DirY {
				cost += 1000 // Turn
			}

			nextLoc := Vec4{curr.Loc.PosX + dir.X, curr.Loc.PosY + dir.Y, dir.X, dir.Y}
			nextNode, _ := visited[nextLoc]

			if maze[nextLoc.PosY][nextLoc.PosX] != '#' {
				if nextNode == nil {
					nextNode = &Node{
						Loc:    nextLoc,
						Parent: make([]*Node, 0, 4),
						Cost:   curr.Cost + cost,
					}
					nextNode.Parent = append(nextNode.Parent, &curr)
					queue = append(queue, *nextNode)
				} else {
					if nextNode.Cost > curr.Cost+cost {
						nextNode.Parent = []*Node{&curr}
						queue = append(queue, *nextNode)
					}
				}
			}
		}
	}
	fmt.Println("Part one answer:", minScore)

	// Part two
	visited2 := make(map[Vec2]bool)
	for i := range paths {
		visited2 = CheckUniqueVisited(&paths[i], visited2)
	}
	fmt.Println("Part two answer:", len(visited2)+1)
}

type Vec2 struct {
	X, Y int
}

type Vec4 struct {
	PosX, PosY,
	DirX, DirY int
}

type Node struct {
	Loc    Vec4
	Parent []*Node
	Cost   int
}

func CheckUniqueVisited(node *Node, visited map[Vec2]bool) map[Vec2]bool {
	for i := range node.Parent {
		p := node.Parent[i]
		visited[Vec2{p.Loc.PosX, p.Loc.PosY}] = true
		visited = CheckUniqueVisited(p, visited)
	}
	return visited
}
