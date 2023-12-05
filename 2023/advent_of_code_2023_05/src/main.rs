use std::{
  fs::File,
  io::{BufRead, BufReader},
};

struct Map {
  source: u64,
  destination: u64,
  range: u64,
}

impl Map {
  pub fn new(data: &String) -> Self {
    let d: Vec<u64> = data
      .split(" ")
      .map(|x| x.trim().parse::<u64>().unwrap())
      .collect();

    return Self {
      source: d[1],
      destination: d[0],
      range: d[2],
    };
  }
}

struct Seed {
  start: u64,
  range: u64,
}

impl Seed {
  pub fn new(start: u64, range: u64) -> Self {
    return Self { start, range };
  }
}

fn main() {
  let mut seeds: Vec<u64> = Vec::new();
  let mut seeds_p2: Vec<Seed> = Vec::new();
  let mut maps: Vec<Vec<Map>> = Vec::new();
  for _i in 0..8 {
    maps.push(Vec::new());
  }

  // Read input
  let mut line: String = String::from("");
  let mut map_index = 0;
  let mut number: u64;
  let mut start = u64::MAX;
  let file = File::open("input.txt").unwrap();
  for l in BufReader::new(file).lines() {
    line.clear();
    line.push_str(&l.unwrap());

    if line.starts_with("seeds:") {
      for num in line[6..].split(' ').map(|x| {
        return x.trim().parse::<u64>();
      }) {
        if num.is_ok() {
          number = num.unwrap();

          // Part one
          seeds.push(number);

          // Part two
          if start != u64::MAX {
            seeds_p2.push(Seed::new(start, number));
            start = u64::MAX;
          } else {
            start = number;
          }
        }
      }
      continue;
    } else if line.starts_with("seed-to-soil map:") {
      map_index = 1;
      continue;
    } else if line.starts_with("soil-to-fertilizer map:") {
      map_index = 2;
      continue;
    } else if line.starts_with("fertilizer-to-water map:") {
      map_index = 3;
      continue;
    } else if line.starts_with("water-to-light map:") {
      map_index = 4;
      continue;
    } else if line.starts_with("light-to-temperature map:") {
      map_index = 5;
      continue;
    } else if line.starts_with("temperature-to-humidity map:") {
      map_index = 6;
      continue;
    } else if line.starts_with("humidity-to-location map:") {
      map_index = 7;
      continue;
    }

    if line.len() == 0 {
      continue;
    }
    maps[map_index].push(Map::new(&line));
  }

  // Part one
  let mut val: u64;
  let mut min_loc = u64::MAX;
  for seed in seeds {
    val = seed;
    for idx in 0..8 {
      val = find_destination(val, &maps[idx]);
    }
    if val < min_loc {
      min_loc = val;
    }
  }
  println!("Part one answer: {min_loc}");

  // Part two
  println!("Part two answer: I don't know");
}

fn find_destination(source: u64, map: &Vec<Map>) -> u64 {
  for m in map {
    if source >= m.source && m.source + m.range >= source {
      return m.destination + (source - m.source);
    }
  }
  return source;
}
