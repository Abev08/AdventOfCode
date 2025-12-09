package main

import (
	"bufio"
	"fmt"
	"math"
	"os"
	"slices"
	"strconv"
	"strings"
)

func main() {
	file, _ := os.Open("input.txt")
	defer file.Close()
	scanner := bufio.NewScanner(file)

	partOne, partTwo := 1, 0
	junctionBoxes := make([]JunctionBox, 0, 10)

	for scanner.Scan() {
		line := scanner.Text()
		if len(line) == 0 {
			continue
		}

		box := JunctionBox{}
		idx := 0
		for f := range strings.SplitSeq(line, ",") {
			switch idx {
			case 0:
				box.pos.X, _ = strconv.ParseFloat(f, 64)
			case 1:
				box.pos.Y, _ = strconv.ParseFloat(f, 64)
			case 2:
				box.pos.Z, _ = strconv.ParseFloat(f, 64)
			}
			idx++
		}
		junctionBoxes = append(junctionBoxes, box)
	}

	circuits := make([]*Circuit, 0, 10)

	step := 0
	for {
		step++
		if step%100 == 0 {
			fmt.Println(step)
		}

		var b1, b2 *JunctionBox
		minDist := math.MaxFloat64
		for i := 0; i < len(junctionBoxes)-1; i++ {
			box1 := &junctionBoxes[i]
			for j := i + 1; j < len(junctionBoxes); j++ {
				box2 := &junctionBoxes[j]

				// Check if boxes are already connected to each other
				if slices.Contains(box1.connection, box2) {
					continue
				}

				dist := box1.pos.dist(&box2.pos)
				if dist < minDist {
					b1, b2 = box1, box2
					minDist = dist
				}
			}
		}

		b1.connection = append(b1.connection, b2)
		b2.connection = append(b2.connection, b1)

		if b1.circuit != nil && b1.circuit == b2.circuit {
			// Nothing, the boxes are already in the same circuit
		} else if b1.circuit == nil && b2.circuit == nil {
			// Boxes doesn't belong to any circuit - create new circuit
			c := &Circuit{}
			circuits = append(circuits, c)
			c.boxes = append(c.boxes, b1)
			c.boxes = append(c.boxes, b2)
			b1.circuit = c
			b2.circuit = c
		} else if b1.circuit != nil && b2.circuit == nil {
			// Box doesn't belong to any circuit, append it to other box circuit
			b1.circuit.boxes = append(b1.circuit.boxes, b2)
			b2.circuit = b1.circuit
		} else if b1.circuit == nil && b2.circuit != nil {
			// Box doesn't belong to any circuit, append it to other box circuit
			b2.circuit.boxes = append(b2.circuit.boxes, b1)
			b1.circuit = b2.circuit
		} else {
			// Both boxes belong to different circuits - connect circuits together
			idx := -1 // Index of box 2 circuit in circuits slice
			for i := range circuits {
				if b2.circuit == circuits[i] {
					idx = i
					break
				}
			}
			if idx < 0 {
				panic("")
			}
			c := b2.circuit
			b1.circuit.boxes = append(b1.circuit.boxes, b2.circuit.boxes...)
			for i := range c.boxes {
				c.boxes[i].circuit = b1.circuit
			}
			b2.circuit = b1.circuit
			circuits = slices.Delete(circuits, idx, idx+1)
		}

		if step == 1000 {
			circuitsLengths := make([]int, len(circuits))
			for i := range circuits {
				circuitsLengths[i] = len(circuits[i].boxes)
			}
			slices.Sort(circuitsLengths)
			for i := range 3 {
				partOne *= circuitsLengths[len(circuitsLengths)-1-i]
			}

			fmt.Println("Part one answer:", partOne)
		}

		if len(circuits) > 0 && len(circuits[0].boxes) == len(junctionBoxes) {
			partTwo = int(b1.pos.X) * int(b2.pos.X)
			fmt.Println("Part two answer:", partTwo)
			break
		}
	}
}

type Vec3 struct {
	X, Y, Z float64
}

func (vec *Vec3) dist(v *Vec3) float64 {
	var distX, distY, distZ float64
	distX = vec.X - v.X
	distX *= distX
	distY = vec.Y - v.Y
	distY *= distY
	distZ = vec.Z - v.Z
	distZ *= distZ
	return distX + distY + distZ
}

type JunctionBox struct {
	pos        Vec3
	circuit    *Circuit
	connection []*JunctionBox
}

type Circuit struct {
	boxes []*JunctionBox
}
