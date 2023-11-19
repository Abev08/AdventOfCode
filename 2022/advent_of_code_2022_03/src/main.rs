use std::{
    fs::File,
    io::{BufRead, BufReader},
};

fn main() {
    // Read the input file
    let mut input: Vec<String> = Vec::new();
    let f = File::open("input.txt");
    if f.is_ok() {
        let f = BufReader::new(f.unwrap());

        for line in f.lines() {
            let l = line.unwrap();
            // println!("{}", l);
            input.push(l);
        }
    } else {
        println!("{}", f.unwrap_err());
        return;
    }

    // Part one
    let mut bags: (&str, &str);
    let mut priority: i32 = 0;
    for line in &input {
        bags = line.split_at(line.len() / 2);

        // Find the common character
        for c in bags.0.chars() {
            if bags.1.find(c).is_some() {
                // println!(
                //     "bag1 {}, len: {}, bag2 {}, len: {} , common char: {}",
                //     bags.0,
                //     bags.0.len(),
                //     bags.1,
                //     bags.1.len(),
                //     c
                // );

                if c.is_uppercase() {
                    priority += (c as i32) - 38;
                } else {
                    priority += (c as i32) - 96;
                }
                break;
            }
        }
    }
    println!("Part one answer: {}", priority);

    // Part two
    priority = 0;
    let mut bags: (&str, &str, &str) = ("", "", "");
    let mut index = 0;
    for line in &input {
        index += 1;
        match index {
            1 => {
                bags.0 = line.as_str();
            }
            2 => {
                bags.1 = line.as_str();
            }
            3 => {
                bags.2 = line.as_str();

                for c in bags.0.chars() {
                    if bags.1.find(c).is_some() {
                        if bags.2.find(c).is_some() {
                            if c.is_uppercase() {
                                priority += (c as i32) - 38;
                            } else {
                                priority += (c as i32) - 96;
                            }
                            break;
                        }
                    }
                }

                index = 0;
            }
            _ => {}
        }
    }
    println!("Part two answer: {}", priority);
}
