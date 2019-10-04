import React, { Component } from 'react'
import axios from 'axios'
import './TodoItem.css'

class TodoItem extends Component {
    constructor(props) {
        super(props);

        this.state = {
            Name: this.props.todoItem.name,
            IsComplete: this.props.todoItem.isComplete
        };
    }

    async onDelete(e) {
        e.preventDefault();

        var response = await axios.post('https://toodoapi220190926085529.azurewebsites.net/api/TodoItems');
        if (response.status === 200) {
            console.log('ok');
        }
    }

    render() {
        return (
            <React.Fragment>
                <form className="todo-item-form">
                    <label className="todo-item-label">{this.state.Name}</label>
                    <button className="todo-item-delete" onClick={this.onDelete}>D</button>
                    <input className="todo-item-checkbox" type="checkbox" defaultChecked={this.state.IsComplete}></input>
                </form>
            </React.Fragment>
        )
    }
}

export default TodoItem;