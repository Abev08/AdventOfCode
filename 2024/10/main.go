package main

import (
	"bufio"
	"fmt"
	"os"
	"slices"
	"strconv"
)

func main() {
	// Open file
	file, err := os.Open("input.txt")
	if err != nil {
		panic(err)
	}
	defer file.Close()

	// Parse the input
	terrain := make([][]int, 0, 100)
	trails := make([]Trail, 0, 100)
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := scanner.Text()
		if len(line) == 0 {
			continue
		}

		row := make([]int, len(line))
		for i, c := range line {
			num := parseInt(c)
			row[i] = num

			if num == 0 {
				trails = append(trails, Trail{
					Start:   Vec2{i, len(terrain)},
					Pos:     Vec2{i, len(terrain)},
					Visited: make([]Vec2, 0, 100),
				})
			}
		}
		terrain = append(terrain, row)
	}

	for {
		done := true
		for i := len(trails) - 1; i >= 0; i-- {
			t := &trails[i]
			if t.Done {
				continue
			}
			t.Value = terrain[t.Pos.Y][t.Pos.X]
			next := t.GetNext(terrain)

			if len(next) == 0 {
				t.Done = true
			} else {
				for j := 1; j < len(next); j++ {
					// Multiple path choices, create copy of current trail
					newT := t.Clone()
					newT.Pos = next[j]
					newT.Visited = append(newT.Visited, next[j])
					trails = append(trails, newT)
				}
				// Move current trail
				t.Pos = next[0]
				t.Visited = append(t.Visited, next[0])
				done = false
			}
		}
		if done {
			break
		}
	}

	// Count different trail ends from the same trail start
	trailCounter := 0
	trailRating := 0
	uniqueTrails := make([]string, 0, 2000)
	starts := make([]Vec2, 0, 1000)
	for i := 0; i < len(trails); i++ {
		st := &trails[i]
		containsStart := false
		for _, s := range starts {
			if st.Start.X == s.X && st.Start.Y == s.Y {
				containsStart = true
				break
			}
		}
		if containsStart {
			continue
		}
		start := Vec2{X: st.Start.X, Y: st.Start.Y}
		starts = append(starts, start)
		ends := make([]Vec2, 0, 100)
		for j := i; j < len(trails); j++ {
			t := trails[j]
			if t.Start.X != start.X || t.Start.Y != start.Y || t.Value != 9 {
				continue
			}
			trailRating++
			tString := fmt.Sprintf("%v", t)
			if !slices.Contains(uniqueTrails, tString) {
				uniqueTrails = append(uniqueTrails, tString)
			}
			containsEnd := false
			for _, e := range ends {
				if e.X == t.Pos.X && e.Y == t.Pos.Y {
					containsEnd = true
					break
				}
			}
			if !containsEnd {
				ends = append(ends, Vec2{X: t.Pos.X, Y: t.Pos.Y})
				trailCounter++
			}
		}
	}

	fmt.Println("Part one answer:", trailCounter)
	fmt.Println("Part two answer using string comparasion:", len(uniqueTrails), ", using counter:", trailRating)
	// FIXME For some reason multiple trails with the same start and visited locations are being generated
}

func parseInt(c rune) int {
	num, err := strconv.ParseInt(string(c), 10, 32)
	if err != nil {
		num = -1
	}
	return int(num)
}

type Vec2 struct {
	X, Y int
}

type Trail struct {
	Start   Vec2
	Visited []Vec2
	Pos     Vec2
	Value   int
	Done    bool
}

func (t *Trail) GetNext(terrain [][]int) []Vec2 {
	next := make([]Vec2, 0, 4)
	if t.Pos.X > 0 && terrain[t.Pos.Y][t.Pos.X-1]-t.Value == 1 { // Left
		next = append(next, Vec2{X: t.Pos.X - 1, Y: t.Pos.Y})
	}
	if t.Pos.X < len(terrain[0])-1 && terrain[t.Pos.Y][t.Pos.X+1]-t.Value == 1 { // Right
		next = append(next, Vec2{X: t.Pos.X + 1, Y: t.Pos.Y})
	}
	if t.Pos.Y > 0 && terrain[t.Pos.Y-1][t.Pos.X]-t.Value == 1 { // Top
		next = append(next, Vec2{X: t.Pos.X, Y: t.Pos.Y - 1})
	}
	if t.Pos.Y < len(terrain)-1 && terrain[t.Pos.Y+1][t.Pos.X]-t.Value == 1 { // Bottom
		next = append(next, Vec2{X: t.Pos.X, Y: t.Pos.Y + 1})
	}
	return next
}

func (t *Trail) Clone() Trail {
	visited := make([]Vec2, len(t.Visited))
	for i, v := range t.Visited {
		visited[i] = Vec2{X: v.X, Y: v.Y}
	}
	return Trail{
		Start:   t.Start,
		Visited: visited,
		Pos:     Vec2{X: t.Pos.X, Y: t.Pos.Y},
		Value:   t.Value,
		Done:    false,
	}
}
