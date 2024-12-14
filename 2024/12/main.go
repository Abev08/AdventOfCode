package main

import (
	"bufio"
	"fmt"
	"os"
)

func main() {
	// Open file
	file, err := os.Open("input.txt")
	if err != nil {
		panic(err)
	}
	defer file.Close()

	// Parse the input
	gardenY := 0
	garden := make([]Plant, 0, 1000)
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := scanner.Text()
		if len(line) == 0 {
			continue
		}

		for x, c := range line {
			garden = append(garden, Plant{
				PlantType: c,
				Position:  Vec2{x, gardenY},
			})
		}
		gardenY++
	}

	plots := make([]Plot, 0, 100)
	for i := range garden {
		plant := &garden[i]
		if plant.Added {
			continue
		}
		plots = append(plots, GetPlot(garden, plant))
	}

	// Part one
	totalCost := 0
	for i := range plots {
		plot := &plots[i]
		plot.GenerateFences()
		totalCost += plot.FenceCount * len(plot.Plants)
	}
	fmt.Println("Part one answer:", totalCost)

	// Part two
	totalCost = 0
	for i := range plots {
		plot := &plots[i]
		plot.ReduceFences()
		totalCost += plot.FenceCount * len(plot.Plants)
	}
	fmt.Println("Part two answer:", totalCost)
}

type Vec2 struct {
	X, Y int
}

type Plant struct {
	PlantType rune
	Position  Vec2
	Added     bool // Is added to a plot
	Fence     []bool
}

type Plot struct {
	PlantType  rune
	Plants     []Plant
	Fences     map[int]map[int][]bool
	FenceCount int
}

func GetPlot(garden []Plant, startPlant *Plant) Plot {
	p := Plot{
		PlantType: startPlant.PlantType,
		Plants:    make([]Plant, 0, 100),
	}
	startPlant.Added = true
	p.Plants = append(p.Plants, *startPlant)
	p.AddAdjacentPlants(garden, startPlant.Position)
	return p
}

func (p *Plot) AddAdjacentPlants(garden []Plant, centerPos Vec2) {
	for i := range garden {
		plant := &garden[i]
		if plant.PlantType != p.PlantType || plant.Added {
			continue
		}
		diffX := abs(plant.Position.X - centerPos.X)
		diffY := abs(plant.Position.Y - centerPos.Y)
		if (diffX == 1 && diffY == 0) || (diffX == 0 && diffY == 1) {
			plant.Added = true
			p.Plants = append(p.Plants, *plant)
			p.AddAdjacentPlants(garden, plant.Position)
		}
	}
}

func (p *Plot) GenerateFences() {
	var directions = []Vec2{{0, -1}, {1, 0}, {0, 1}, {-1, 0}} // Up, Right, Down, Left
	// Generate fences map
	fences := make(map[int]map[int][]bool)
	for _, plant := range p.Plants {
		if _, ok := fences[plant.Position.Y]; !ok {
			fences[plant.Position.Y] = make(map[int][]bool)
		}
		fences[plant.Position.Y][plant.Position.X] = make([]bool, 4) // Up, Right, Down, Left
	}
	// Generate fences
	fenceCount := 0
	for _, plant := range p.Plants {
		for i, dir := range directions {
			_, ok := fences[plant.Position.Y+dir.Y][plant.Position.X+dir.X]
			if ok {
				continue // There is another plant in current direction - skip
			}
			// Else generate fence
			fences[plant.Position.Y][plant.Position.X][i] = true
			fenceCount++
		}
	}

	p.Fences = fences
	p.FenceCount = fenceCount
}

func (p *Plot) ReduceFences() {
	var dist int
	for _, plant := range p.Plants {
		fence := p.Fences[plant.Position.Y][plant.Position.X]
		for i, present := range fence {
			if !present {
				continue
			}
			switch i {
			case 0, 2: // Up, Down -> check plants on left and right
				dist = 1
				for {
					f, ok := p.Fences[plant.Position.Y][plant.Position.X+dist]
					if !ok || !f[i] {
						break
					}
					p.Fences[plant.Position.Y][plant.Position.X+dist][i] = false // Combine fence
					p.FenceCount--
					dist++
				}
				dist = 1
				for {
					f, ok := p.Fences[plant.Position.Y][plant.Position.X-dist]
					if !ok || !f[i] {
						break
					}
					p.Fences[plant.Position.Y][plant.Position.X-dist][i] = false // Combine fence
					p.FenceCount--
					dist++
				}
			case 1, 3: // Left, Right -> check plants up and down
				dist = 1
				for {
					f, ok := p.Fences[plant.Position.Y+dist][plant.Position.X]
					if !ok || !f[i] {
						break
					}
					p.Fences[plant.Position.Y+dist][plant.Position.X][i] = false // Combine fence
					p.FenceCount--
					dist++
				}
				dist = 1
				for {
					f, ok := p.Fences[plant.Position.Y-dist][plant.Position.X]
					if !ok || !f[i] {
						break
					}
					p.Fences[plant.Position.Y-dist][plant.Position.X][i] = false // Combine fence
					p.FenceCount--
					dist++
				}
			}
		}
	}
}

func abs(val int) int {
	if val < 0 {
		return -val
	}
	return val
}
