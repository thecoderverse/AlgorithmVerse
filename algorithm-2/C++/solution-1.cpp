// Usage: g++ -Ofast solution-1.cpp -o solution && .\solution

#include <array>
#include <bitset>
#include <iostream>
#include <memory>
#include <numeric>
#include <vector>
#include <stack>

enum BoardStatus {
    NOT_FINISHED,
    FINISHED,
    ILLEGAL
};

class Board {
private:
    std::array<uint8_t, 81> cells;

public:
    Board(std::array<uint8_t, 81> cells): cells{cells} {}

    void set_cell(uint8_t cell, uint8_t val) { 
        cells[cell] = val; 
    }

    bool is_cell_empty(uint8_t cell) {
        return !cells[cell];
    }

    BoardStatus check_board() const {
        auto status = FINISHED;
        
        std::bitset<81> 
            lateral_set,
            vertical_set,
            sub_set;

        for (uint8_t index = 0; index < 81; index++) {
            uint8_t 
                x = index % 9,
                y = index / 9,
                val = cells[index] - 1; // 0 is empty, 0 - 1 = 255 (unsigned)

            if (val > 9) {
                status = NOT_FINISHED;
                continue;
            }

            uint8_t
                lateral_set_index = y * 9 + val,
                vertical_set_index = val * 9 + x,
                sub_set_index = ((y / 3 * 3) + val / 3) * 9 + ((x / 3 * 3) + val % 3);

            if (
                lateral_set.test(lateral_set_index) ||
                vertical_set.test(vertical_set_index) ||
                sub_set.test(sub_set_index) 
            ) return ILLEGAL;

            lateral_set.set(lateral_set_index);
            vertical_set.set(vertical_set_index);
            sub_set.set(sub_set_index);
        }

        return status;
    }

    void print() const {
        for (uint8_t index = 0; index < 81; index++) {
            int val = static_cast<int>(cells[index]);
            std::cout << " " << (val ? val : ' ') << " ";
            if (index % 9 == 8) std::cout << "\n";
        }
    }
};

class BoardStack {
private:
    std::shared_ptr<Board> board;
    std::stack<std::pair<uint8_t, uint8_t>> move_stack; // std::pair<first: cell, second: val>

public:
    BoardStack(std::shared_ptr<Board> board): board{board} {}

    void make_move(uint8_t cell, uint8_t val) {
        board->set_cell(cell, val);
        move_stack.push({cell, val});
    }

    std::pair<uint8_t, uint8_t> undo_move() {
        auto prev_move = move_stack.top();
        board->set_cell(prev_move.first, 0);
        move_stack.pop();
        return prev_move;
    }

    bool is_empty() const {
        return move_stack.empty();
    }
};

class SudokuSolver {
private:
    std::shared_ptr<Board> board;
    BoardStack board_stack;
    std::vector<uint8_t> empty_cells;

public:
    SudokuSolver(std::shared_ptr<Board> board): 
        board{board},
        board_stack{BoardStack(board)} {
        init_empty_cells();
    }

    BoardStatus solve() {
        BoardStatus status;

        uint8_t current_empty_cell = 0, last_value = 0;
        while ((status = board->check_board()) != FINISHED) {
            if (status == ILLEGAL || last_value == 9) {
                if (board_stack.is_empty()) break;
                
                auto last_move = board_stack.undo_move();
                last_value = last_move.second;
                current_empty_cell--;
                continue;
            }

            board_stack.make_move(empty_cells[current_empty_cell++], last_value + 1);
            last_value = 0;
        }
        
        return status;
    }

private:
    void init_empty_cells() {
        for (uint8_t index = 0; index < 81; index++) {
            if (board->is_cell_empty(index)) empty_cells.push_back(index);
        }
    }
};

std::array<uint8_t, 81> parse() {
    std::array<uint8_t, 81> cells;
    
    for (uint8_t index = 0; index < 81;) {
        char ch;
        std::cin >> ch;

        if (!isdigit(ch)) continue;

        cells[index++] = ch - '0';
    }
    
    return cells;
}

void solve() {
    auto cells = parse();
    auto board = std::make_shared<Board>(Board(cells));

    if (board->check_board() == ILLEGAL) {
        std::cout << "Initial board is in an illagel state!\n";
        return;
    }

    SudokuSolver(board).solve();
    if (board->check_board() == NOT_FINISHED) {
        std::cout << "The board is not solveable!\n";
        return;
    }

    board->print();
}

int main(void) {
    solve();
}