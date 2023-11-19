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
    let mut food_val: i32;
    let mut total: i32 = 0;
    let mut max: i32 = 0;
    for food in &input {
        if food.len() > 0 {
            food_val = food.parse().unwrap();
            total += food_val;
        } else {
            if total > max {
                max = total;
            }
            total = 0;
        }
    }
    if total > max {
        max = total;
    }
    println!("Part one answer: {}", max);

    // Part two
    total = 0;
    let mut max = [0, 0, 0];
    for food in &input {
        if food.len() > 0 {
            food_val = food.parse().unwrap();
            total += food_val;
        } else {
            if total > max[0] {
                max[0] = total;
            }
            max.sort();
            total = 0;
        }
    }
    if total > max[0] {
        max[0] = total;
    }
    println!("Part two answer: {}", max[0] + max[1] + max[2]);

    // Press any key to continue prompt
    // std::process::Command::new("cmd.exe")
    //     .arg("/c")
    //     .arg("pause")
    //     .status()
    //     .unwrap();
}
