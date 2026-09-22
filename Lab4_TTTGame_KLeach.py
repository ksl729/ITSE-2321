#Name: Lab4_KLeach
#Date: 09/22/2026
#Description: Tic Tac Toe Game Code Structure

import random

class TicTacToeGame:
	# __init__ meets is the constructor initializing the board
    def __init__(self):
		# Attribute to set the gameboard to a 3x3 grid
        self.board = [[' ' for _ in range(3)] for _ in range(3)]
        self.current_player = 'X'
		# Creates the lines on the board
    def display_board(self):
        for row in self.board:
            print('|'.join(row))
            print('-' * 5)

    def process_input(self, row, col):
        # Processes player's selection
        if self.board[row][col] == ' ':
            self.board[row][col] = self.current_player
            return True
		# If the spot is taken, player cannot select spot
        return False

    def computer_move(self):
        # Manages the computer's selection
        empty_spot = [(r, c) for r in range(3) for c in range(3) if self.board
[r][c] == ' ']
		#If the spot is empty, computer can select spot
        if empty_spot:
            row, col = random.choice(empty_spot)
            self.board[row][col] = 'O'

    def check_winner(self):
        # Checks if a player has won
        lines = []

        # First, checks the rows and columns - [r][c]
        for i in range(3):
            lines.append(self.board[i])
            lines.append([self.board[0][i], self.board[1][i], self.board[2][i]])

        # Then, checks for diagonals
        lines.append([self.board[0][0], self.board[1][1], self.board[2][2]])
        lines.append([self.board[0][2], self.board[1][1], self.board[2][0]])
		# This piece handles if a player has won
        for line in lines:
            if line[0] != ' ' and line[0] == line[1] == line[2]:
                return line[0]  # Displays which player has won - 'X' or 'O'

        return None

# Name: Lab4_KLeach
# Date: 09/22/2026
# Description: Tic Tac Toe Game Code Structure

import random
import tkinter as tk


class TicTacToeGame:
    def __init__(self):
        self.board = [[' ' for _ in range(3)] for _ in range(3)]
        self.current_player = 'X'

    def reset_game(self):
        self.board = [[' ' for _ in range(3)] for _ in range(3)]
        self.current_player = 'X'

    def make_move(self, row, col):
        if 0 <= row < 3 and 0 <= col < 3 and self.board[row][col] == ' ':
            self.board[row][col] = self.current_player
            return True
        return False

    def check_winner(self):
        lines = []
        for i in range(3):
            lines.append(self.board[i])
            lines.append([self.board[0][i], self.board[1][i], self.board[2][i]])

        lines.append([self.board[0][0], self.board[1][1], self.board[2][2]])
        lines.append([self.board[0][2], self.board[1][1], self.board[2][0]])

        for line in lines:
            if line[0] != ' ' and line[0] == line[1] == line[2]:
                return line[0]

        if all(cell != ' ' for row in self.board for cell in row):
            return 'Tie'

        return None


class TicTacToeGUI:
    def __init__(self, root):
        self.root = root
        self.root.title("Tic Tac Toe - Lab 4")
        self.game = TicTacToeGame()
        self.single_player_mode = False

        self.mode_frame = tk.Frame(self.root)
        self.mode_frame.pack(pady=10)

        self.mp_btn = tk.Button(self.mode_frame, text="Multiplayer", command=self.set_multiplayer)
        self.mp_btn.pack(side=tk.LEFT, padx=5)

        self.sp_btn = tk.Button(self.mode_frame, text="Single Player", command=self.set_singleplayer)
        self.sp_btn.pack(side=tk.LEFT, padx=5)

        self.board_frame = tk.Frame(self.root)
        self.board_frame.pack()

        self.buttons = [[None for _ in range(3)] for _ in range(3)]
        for r in range(3):
            for c in range(3):
                btn = tk.Button(
                    self.board_frame,
                    text="",
                    font=('normal', 20, 'bold'),
                    width=5,
                    height=2,
                    command=lambda r=r, c=c: self.on_click(r, c)
                )
                btn.grid(row=r, column=c)
                self.buttons[r][c] = btn

        self.status_label = tk.Label(self.root, text="Select Mode to Start", font=('normal', 12))
        self.status_label.pack(pady=10)

        self.reset_btn = tk.Button(self.root, text="Reset Game", command=self.reset_ui)
        self.reset_btn.pack(pady=5)

    def set_multiplayer(self):
        self.single_player_mode = False
        self.reset_ui()
        self.status_label.config(text="Mode: Multiplayer (X starts)")

    def set_singleplayer(self):
        self.single_player_mode = True
        self.reset_ui()
        self.status_label.config(text="Mode: Single Player (X starts)")

    def reset_ui(self):
        self.game.reset_game()
        for r in range(3):
            for c in range(3):
                self.buttons[r][c].config(text="", state="normal", relief="raised")

    def on_click(self, r, c):
        if self.single_player_mode and self.game.current_player == 'O':
            return

        if self.game.make_move(r, c):
            self.update_button(r, c)
            self.check_game_over()

            if self.single_player_mode and self.game.current_player == 'O':
                self.root.after(500, self.computer_move)

    def update_button(self, r, c):
        self.buttons[r][c].config(text=self.game.board[r][c], state="disabled", relief="sunken")

    def computer_move(self):
        empty_spots = []
        for r in range(3):
            for c in range(3):
                if self.game.board[r][c] == ' ':
                    empty_spots.append((r, c))

        if empty_spots:
            r, c = random.choice(empty_spots)
            if self.game.make_move(r, c):
                self.update_button(r, c)
                self.check_game_over()

    def check_game_over(self):
        winner = self.game.check_winner()
        if winner:
            if winner == "Tie":
                self.status_label.config(text="It's a Tie!")
            else:
                self.status_label.config(text=f"{winner} Wins!")

            for r in range(3):
                for c in range(3):
                    self.buttons[r][c].config(state="disabled")
        else:
            self.game.current_player = 'O' if self.game.current_player == 'X' else 'X'
            self.status_label.config(text=f"{self.game.current_player}'s turn")


if __name__ == "__main__":
    root = tk.Tk()
    app = TicTacToeGUI(root)
    root.mainloop()