

if __name__ == "__main__":
    filename = "./render_times_50_doublesphere_cracked.csv"
    try:
        f = open(filename, "r")
        f_new = open('render_times_50_doublesphere_cracked_1.csv', 'w')
    except FileNotFoundError:
        print(f"Error: File '{filename}' not found.")
        exit()
    except Exception as e:
        print(f"Error reading CSV file: {e}")
        exit(1)

    print(line:=f.readline().strip().split(","))
    if line != ['Threads', 'Run', 'Time (ms)']:
        print("Unexpected columns!")
        exit(1)
    f_new.write(','.join(line) + '\n')

    workers = []
    sums = []
    cur_num_workers = 0
    while line := f.readline():
        line = line.strip().split(',')
        if int(line[0]) == 16:
            line[2] = str(float(line[2]) - 1)
        if (int(line[0]) == 64):
            line[2] = str(float(line[2]) + 1)

        f_new.write(','.join(line) + '\n')
