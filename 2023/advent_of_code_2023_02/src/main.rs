use std::{fs::File, io::BufRead, io::BufReader};

const MAX_RED: i32 = 12;
const MAX_GREEN: i32 = 13;
const MAX_BLUE: i32 = 14;

fn main() {
  // Read input
  let mut input: Vec<String> = Vec::new();
  let file = File::open("input.txt").unwrap();
  for line in BufReader::new(file).lines() {
    input.push(line.unwrap());
  }

  let mut sum = 0;
  let mut sum2 = 0;
  let mut start: usize;
  let mut end: usize;
  let mut possible: bool;
  let mut min_red: i32;
  let mut min_green: i32;
  let mut min_blue: i32;
  for line in input {
    possible = true;
    min_red = 0;
    min_green = 0;
    min_blue = 0;
    end = line.find(':').unwrap() + 1;

    // Check the revealed ballss
    loop {
      if let Some(idx) = line[end..].find(';') {
        start = end;
        end += idx;
      } else {
        start = end;
        end = line.len();
      }

      let game = check_possibility(&line[start..end].trim());

      // Part one
      possible &= game.0;

      // Part two
      if game.1 > min_red {
        min_red = game.1;
      }
      if game.2 > min_green {
        min_green = game.2;
      }
      if game.3 > min_blue {
        min_blue = game.3;
      }

      if end == line.len() {
        break;
      } else {
        end += 1;
      }
    }

    // Part one sum
    if possible {
      // Get the game number
      start = line.find(' ').unwrap() + 1;
      end = line.find(':').unwrap();
      sum += line[start..end].parse::<i32>().unwrap();
    }

    // Part two sum
    sum2 += min_red * min_green * min_blue;
  }

  println!("Part one answer: {}", sum);
  println!("Part two answer: {}", sum2);
}

fn check_possibility(game: &str) -> (bool, i32, i32, i32) {
  let mut val_red = 0;
  let mut val_green = 0;
  let mut val_blue = 0;
  let mut start: usize;
  let mut end: usize = 0;
  let mut separator: usize;
  let mut color: &str;
  let mut game_possible = true;

  loop {
    if let Some(idx) = game[end..].find(',') {
      start = end;
      end += idx;
    } else {
      start = end;
      end = game.len();
    }

    separator = start + game[start..end].find(' ').unwrap();
    color = game[(separator + 1)..end].trim();
    if color == "red" {
      val_red += game[start..separator].parse::<i32>().unwrap();
      if val_red > MAX_RED {
        game_possible = false;
      }
    } else if color == "green" {
      val_green += game[start..separator].parse::<i32>().unwrap();
      if val_green > MAX_GREEN {
        game_possible = false;
      }
    } else if color == "blue" {
      val_blue += game[start..separator].parse::<i32>().unwrap();
      if val_blue > MAX_BLUE {
        game_possible = false;
      }
    }

    if end == game.len() {
      break;
    } else {
      end += 2;
    }
  }

  return (game_possible, val_red, val_blue, val_green);
}
