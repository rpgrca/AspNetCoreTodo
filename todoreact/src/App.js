import React, { Component } from 'react';
import './App.css';
import TodoFrame from './components/TodoFrame/TodoFrame';

class App extends Component {
    render() {
        return (
          <div className="App">
              <TodoFrame />
          </div>
        );
    }
}

export default App;
