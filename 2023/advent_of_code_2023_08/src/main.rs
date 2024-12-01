use std::{
  fs::File,
  io::{BufRead, BufReader},
};

#[derive(Debug)]
struct Node {
  name: String,
  next_l_name: String,
  next_r_name: String,
}

impl Node {
  pub fn new(data: &String) -> Node {
    let mut n = Node {
      name: String::new(),
      next_l_name: String::new(),
      next_r_name: String::new(),
    };

    let mut sep1 = data.find('=').unwrap();
    n.name.push_str(data[..sep1].trim());

    sep1 = data.find('(').unwrap() + 1;
    let sep2 = data.find(',').unwrap();
    n.next_l_name.push_str(data[sep1..sep2].trim());
    sep1 = data.rfind(')').unwrap();
    n.next_r_name.push_str(data[(sep2 + 1)..sep1].trim());

    return n;
  }
}

fn main() {
  // Read input
  let mut directions: Vec<char> = Vec::new();
  let mut current: usize = 0;
  let mut nodes: Vec<Node> = Vec::new();
  {
    let mut line = String::new();
    let mut read_nodes = false;
    let file = File::open("input.txt").unwrap();
    for l in BufReader::new(&file).lines() {
      line.clear();
      line.push_str(&l.unwrap());
      if line.len() == 0 {
        continue;
      }

      if read_nodes {
        let n = Node::new(&line);
        if n.name == "AAA" {
          current = nodes.len();
        }
        nodes.push(n);
      } else {
        directions.append(&mut line.trim().chars().collect());
        read_nodes = true;
      }
    }
  }

  // Part one
  let mut steps = 0;
  let mut dir: usize = 0;
  let mut next: &String;
  loop {
    steps += 1;
    // Get next node name
    if directions[dir] == 'L' {
      next = &nodes[current].next_l_name;
    } else {
      next = &nodes[current].next_r_name;
    }

    if next == "ZZZ" {
      break;
    }

    // Find next node index
    for i in 0..nodes.len() {
      if nodes[i].name == *next {
        current = i;
        break;
      }
    }

    dir = (dir + 1) % directions.len();
  }
  println!("Part one answer: {steps}");

  // Part two, doesn't work?
  let mut current_nodes: Vec<&Node> = Vec::new();
  steps = 0;
  dir = 0;
  let mut reached_end: bool;
  for i in 0..nodes.len() {
    if nodes[i].name.ends_with('A') {
      current_nodes.push(&nodes[i]);
    }
  }
  for i in 0..current_nodes.len() {
    println!("{:?}", current_nodes[i]);
  }
  loop {
    steps += 1;
    // Get next node name
    if directions[dir] == 'L' {
      for i in 0..current_nodes.len() {
        for ii in 0..nodes.len() {
          if current_nodes[i].next_l_name == nodes[ii].name {
            current_nodes.remove(i);
            current_nodes.insert(i, &nodes[ii]);
            break;
          }
        }
      }
    } else {
      for i in 0..current_nodes.len() {
        for ii in 0..nodes.len() {
          if current_nodes[i].next_r_name == nodes[ii].name {
            current_nodes.remove(i);
            current_nodes.insert(i, &nodes[ii]);
            break;
          }
        }
      }
    }

    if steps % 10000 == 0 {
      println!("{steps}");
      //   println!();
      //   for i in 0..current_nodes.len() {
      //     println!("{:?}", current_nodes[i]);
      //   }
    }

    reached_end = true;
    for i in 0..current_nodes.len() {
      if !current_nodes[i].name.ends_with('Z') {
        reached_end = false;
        break;
      }
    }
    if reached_end {
      break;
    }

    dir = (dir + 1) % directions.len();
  }
  println!("Part two answer: {steps}");
}
