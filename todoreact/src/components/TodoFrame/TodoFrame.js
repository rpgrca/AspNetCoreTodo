import React, { Component } from 'react'
import axios from 'axios';
import TodoItem from '../TodoItem/TodoItem'
import TodoAddItem from '../TodoAddItem/TodoAddItem'
import './TodoFrame.css'

class TodoFrame extends Component {
    state = {
        todoItems: null
    }

    async componentDidMount() {
        var response = await axios.get('https://toodoapi220190926085529.azurewebsites.net/api/TodoItems');
        if (response.status === 200) {
            console.log('ok');
        }
        this.setState({
            todoItems: response.data
        });
    }

    render() {
        if (this.state.todoItems) {
                var items = this.state.todoItems.map(todoItem => (
                    <TodoItem key={todoItem.id} todoItem={todoItem} />
                ));

            return (
                <div className="todo-frame-container">
                    { items }
                    <TodoAddItem />
                </div>
            );
        }
        else {
            return (
                <div>
                    Empty TodoItem list
                    <TodoAddItem />
                </div>
            );
        }
    }
}

export default TodoFrame;