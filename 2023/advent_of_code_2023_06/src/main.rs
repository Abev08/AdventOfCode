use std::{
  fs::File,
  io::{BufRead, BufReader},
};

fn main() {
  // Read input
  let mut races: Vec<(u64, u64)> = Vec::new();
  let file = File::open("input.txt").unwrap();
  let mut last: usize = 0;
  for l in BufReader::new(file).lines() {
    if l.is_ok() {
      let line = l.unwrap();
      if line.starts_with("Time") {
        let separator = line.find(':').unwrap() + 1;
        for time in line[separator..].split(' ') {
          if time.len() > 0 {
            races.push((time.parse().unwrap(), 0));
          }
        }

        // Part two
        races.push((
          line[separator..].replace(' ', "").trim().parse().unwrap(),
          0,
        ));
      } else if line.starts_with("Distance") {
        let separator = line.find(':').unwrap() + 1;
        let mut race_idx = 0;
        for dist in line[separator..].split(' ') {
          if dist.len() > 0 {
            races[race_idx].1 = dist.parse().unwrap();
            race_idx += 1;
          }
        }

        // Part two
        last = races.len() - 1;
        races[last].1 = line[separator..].replace(' ', "").trim().parse().unwrap();
      }
    }
  }

  // Part one
  let mut dist: u64;
  let mut win: u32;
  let mut sum = 1;
  let mut idx: usize = 0;
  let mut sum2: u32 = 0;
  for race in &races {
    if idx == last {
      // Part two
      for hold in 0..race.0 {
        dist = (race.0 - hold) * hold;
        if dist > race.1 {
          sum2 += 1;
        }
      }
    } else {
      win = 0;
      for hold in 0..race.0 {
        dist = (race.0 - hold) * hold;
        if dist > race.1 {
          win += 1;
        }
      }
      sum *= win;
    }
    idx += 1;
  }
  println!("Part one answer: {sum}");
  println!("Part two answer: {sum2}");
}
